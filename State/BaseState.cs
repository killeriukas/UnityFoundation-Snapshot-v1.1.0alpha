using System;
using TMI.Core;
using TMI.LogManagement;
using TMI.Notification;
using TMI.TimeManagement;

namespace TMI.State {

	public abstract class BaseState : BaseNotificationObject, IState {

        private IState _nextState = null;
		public IState nextState {
            get {
                return _nextState;
            }
            protected set {
                Assert.IsNull(value, "Next state cannot be set to null!");

                if(_nextState != null) {
                    Type nextStateType = _nextState.GetType();
                    Type newNextStateType = value.GetType();
                    Logging.LogWarning(this, "Resetting the next state [{0}] into the new next state [{1}].", nextStateType.Name, newNextStateType.Name);
                }

                _nextState = value;
            }
        }

        private GameTime enteredStateTime;

        protected TimeSpan timeInState {
            get {
                GameTime now = GameTime.now;
                TimeSpan diff = now - enteredStateTime;
                return diff;
            }
        }

		protected BaseState(IInitializer initializer) : base(initializer) {
		}

		public virtual void Enter() {
            enteredStateTime = GameTime.now;
        }

        public virtual void Update() {

        }

        public virtual void Exit() {
			
		}

    }

}