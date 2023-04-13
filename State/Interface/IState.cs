using System;
namespace TMI.State {

	public interface IState : IDisposable {
        IState nextState { get; }
		void Enter();
		void Update();
		void Exit();
	}

}