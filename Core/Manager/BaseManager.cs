using System;
using TMI.LogManagement;
using UnityEngine;

namespace TMI.Core {

	public abstract class BaseManager : UnityBehaviour, IManager, IUpdatable {

        private IExecutionManager executionManager;

        sealed public override void Setup(IInitializer initializer) {
            throw new InvalidOperationException("All managers have to call Setup(IInitializer, bool) instead.");
        }

        public virtual void Setup(IInitializer initializer, bool isNew) {
            base.Setup(initializer);
            executionManager = initializer.GetManager<ExecutionManager, IExecutionManager>();
        }

        protected void RegisterUpdate(ExecutionManager.Priority priority = ExecutionManager.Priority.Priority_1) {
            executionManager.Register(this, OnUpdate, priority);
        }

        protected virtual ExecutionManager.Result OnUpdate() {
            return ExecutionManager.Result.Finish;
        }

        protected void UnregisterUpdate(ExecutionManager.Priority priority = ExecutionManager.Priority.Priority_1) {
            executionManager.Unregister(this, priority);
        }

        public virtual void OnPreDestroy() {
        }

    }

}