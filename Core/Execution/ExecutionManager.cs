using System;
using System.Collections.Generic;
using TMI.Collection;
using TMI.LogManagement;

namespace TMI.Core {

    [Loggable]
    public class ExecutionManager : BaseManager, IExecutionManager {

        public enum Result {
            Continue,
            Finish
        }

        public enum Priority {
            Priority_1 = 0,
            Priority_2,
            Priority_3,
            Priority_4,
            Priority_5
        }

        private readonly List<Priority> priorityList = new List<Priority>();

        protected override void Awake() {
            base.Awake();

            priorityList.Add(Priority.Priority_1);
            priorityList.Add(Priority.Priority_2);
            priorityList.Add(Priority.Priority_3);
            priorityList.Add(Priority.Priority_4);
            priorityList.Add(Priority.Priority_5);
        }

        private class Updatable {
            public readonly DeferredDictionary<IUpdatable, Func<Result>> functionByUpdatableInterface = new DeferredDictionary<IUpdatable, Func<Result>>();
        }

        private readonly Dictionary<Priority, Updatable> updatableByPriority = new Dictionary<Priority, Updatable>();

        private void Update() {
            foreach(Priority priority in priorityList) {
                if(updatableByPriority.ContainsKey(priority)) {
                    Updatable updatable = updatableByPriority[priority];

                    //apply changes before scanning the dictionary
                    updatable.functionByUpdatableInterface.ApplyChanges();

                    foreach(var kvp in updatable.functionByUpdatableInterface) {
                        Result result = kvp.Value();
                        if(result == Result.Finish) {
                            Logging.Log(this, "Updatable [{0}] with method [{1}] in priority [{2}] has finished execution.", kvp.Key.GetType().Name, kvp.Value.Method.Name, priority);
                            updatable.functionByUpdatableInterface.Remove(kvp.Key);
                        }
                    }

                    //apply changes after scanning the dictionary
                    updatable.functionByUpdatableInterface.ApplyChanges();

                    if(0 == updatable.functionByUpdatableInterface.Count) {
                        updatableByPriority.Remove(priority);
                    }
                }
            }
        }

        public void Register(IUpdatable updatableRequest, Func<Result> updateMethod, Priority priority = Priority.Priority_1) {
            string updatableRequestName = updatableRequest.GetType().Name;
        
            Logging.Log(this, "Registering updatable [{0}] with method [{1}] in priority [{2}].", updatableRequestName, updateMethod.Method.Name, priority);

            Updatable updatable;
            if(!updatableByPriority.TryGetValue(priority, out updatable)) {
                updatable = new Updatable();
                updatableByPriority.Add(priority, updatable);
            }

            updatable.functionByUpdatableInterface.Add(updatableRequest, updateMethod);
            
            Logging.Log(this, "Registered updatable [{0}] with method [{1}] in priority [{2}].", updatableRequestName, updateMethod.Method.Name, priority);
        }

        public void Unregister(IUpdatable updatableRequest, Priority priority = Priority.Priority_1) {
            string updatableRequestName = updatableRequest.GetType().Name;
            
            Logging.Log(this, "Unregistering all methods in updatable [{0}] in priority [{1}].", updatableRequestName, priority);
            Updatable updatable = updatableByPriority[priority];
            updatable.functionByUpdatableInterface.Remove(updatableRequest);
            Logging.Log(this, "Unregistered all methods in updatable [{0}] in priority [{1}].", updatableRequestName, priority);
        }

        protected override void OnDestroy() {
            Logging.Log(this, "Clearing all updatable methods in ExecutionManager.");
            updatableByPriority.Clear();
            base.OnDestroy();
        }
        
    }


}