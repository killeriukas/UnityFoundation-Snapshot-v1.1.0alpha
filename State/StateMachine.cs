using System;
using TMI.Helper;
using TMI.LogManagement;

namespace TMI.State {

	[Loggable]
    public class StateMachine : IStateMachine {
	
		private IState currentState;

		public static IStateMachine Create(IState initialState) {
			return new StateMachine(initialState);
		}

		private StateMachine(IState initialState) {
            EnterNewState(initialState);
		}

		public void Update() {
            IState newState = currentState.nextState;

            //if the next state is set before the update loop - we shall skip the update loop
            if(newState == null) {
                currentState.Update();
                newState = currentState.nextState;
            }

			if(newState != null) {
				currentState.Exit();
				currentState.Dispose();

                EnterNewState(newState);
			}
		}

        private void EnterNewState(IState newState) {
            Type newStateType = newState.GetType();

            if(currentState != null) {
                Assert.IsTrue<ArgumentException>(currentState.GetType() == newStateType,
                "Cannot change the state to itself. State name: " + newStateType.Name);
            }

            Logging.Log(this, "Changing to a new state [{0}].", newStateType.Name);

            currentState = newState;
            currentState.Enter();
        }

		public void Dispose() {
			currentState.Exit();
			GeneralHelper.DisposeAndMakeDefault(ref currentState);
		}
	}

}