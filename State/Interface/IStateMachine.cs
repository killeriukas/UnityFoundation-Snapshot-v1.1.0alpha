using System;

namespace TMI.State {
	
	public interface IStateMachine : IDisposable {
		void Update();
	}

}