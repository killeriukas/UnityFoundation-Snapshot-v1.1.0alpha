using System;

namespace TMI.Core {
    public interface IExecutionManager : IManager {
        void Register(IUpdatable updatableRequest, Func<ExecutionManager.Result> updateMethod, ExecutionManager.Priority priority = ExecutionManager.Priority.Priority_1);
        void Unregister(IUpdatable updatableRequest, ExecutionManager.Priority priority = ExecutionManager.Priority.Priority_1);
    }
}

