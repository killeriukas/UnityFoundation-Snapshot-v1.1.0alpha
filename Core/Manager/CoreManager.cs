using System;
using System.Collections.Generic;
using TMI.Helper;
using UnityEngine;

namespace TMI.Core {

    public class CoreManager : MonoBehaviour {

        private class ManagerAcquirer : IAcquirer {
            private CoreManager coreManager;

            public ManagerAcquirer(CoreManager coreManager) {
                this.coreManager = coreManager;
            }

            public TManagerInterface AcquireManager<TManagerInterface>(Type managerType) where TManagerInterface : IManager {
                return (TManagerInterface)coreManager.AcquireManager(managerType);
            }
            
            public TManagerInterface AcquireManager<TManager, TManagerInterface>() where TManager : BaseManager, TManagerInterface where TManagerInterface : IManager {
                return coreManager.AcquireManager<TManager>();
            }
        }

        private static bool created = false;
        private static readonly string coreName = typeof(CoreManager).Name;


        private IAcquirer acquirer = null;
        private Dictionary<Type, BaseManager> managerByType = new Dictionary<Type, BaseManager>();

        private Dictionary<Type, bool> managerStateByType = new Dictionary<Type, bool>();
        private HashSet<Type> managerRecycleBin;

        private void Awake() {
            created = true;
        }

        public static CoreManager Create() {
            if(created) {
                CoreManager coreManager = GameObject.FindObjectOfType<CoreManager>();
                return coreManager;
            } else {
                GameObject go = new GameObject(coreName);
                CoreManager coreManager = go.AddComponent<CoreManager>();
                return coreManager;
            }
        }

        public IAcquirer StartAcquire() {
            Assert.IsNotNull(acquirer, "Cannot acquire more than one acquirer! Have you forgot to EndAcquire()?");

            managerRecycleBin = new HashSet<Type>(managerByType.Keys);

            acquirer = new ManagerAcquirer(this);
            return acquirer;
        }

        public void EndAcquire(IAcquirer acquirer, IInitializer initializer) {
            Assert.IsFalse<ArgumentException>(this.acquirer == acquirer, "Returning the wrong acquirer!");
            this.acquirer = null;

            //delete all leftovers
            foreach(Type type in managerRecycleBin) {
                BaseManager manager = managerByType[type];
                manager.OnPreDestroy();
            }

            foreach(Type type in managerRecycleBin) {
                BaseManager manager = managerByType[type];
                managerByType.Remove(type);
                GameObject.Destroy(manager.gameObject);
            }
            managerRecycleBin.Clear();

            //initialize new managers
            foreach(Type type in managerByType.Keys) {
                bool isNewlyCreated = managerStateByType[type];
                managerByType[type].Setup(initializer, isNewlyCreated);
            }
            managerStateByType.Clear();
        }

        private IManager AcquireManager(Type managerType) {
            if(managerByType.ContainsKey(managerType)) {
                managerRecycleBin.Remove(managerType);
                BaseManager manager = (BaseManager)GetManager(managerType);
                managerStateByType.Add(managerType, false);
                return manager;
            } else {
                managerStateByType.Add(managerType, true);
                GameObject newManagerGo = new GameObject(managerType.Name);
                newManagerGo.transform.SetParent(base.transform, false);
                BaseManager newManager = (BaseManager)newManagerGo.AddComponent(managerType);
                managerByType.Add(managerType, newManager);
                return newManager;
            }
            
        }
        
        private TManager AcquireManager<TManager>() where TManager : BaseManager {
            Type managerType = typeof(TManager);
            return (TManager)AcquireManager(managerType);
        }

        public TManagerInterface GetManager<TManager, TManagerInterface>() where TManager : BaseManager, TManagerInterface, new() where TManagerInterface : class, IManager {
            Type managerType = typeof(TManager);
            TManager manager = (TManager)managerByType[managerType];
            return manager;
        }

        private IManager GetManager(Type managerType) {
            List<Type> allAssignableTypes = ReflectionHelper.FindAllAssignableClassTypesForClass(managerType);
            
            Assert.IsTrue<InvalidOperationException>(allAssignableTypes.Count == 0, "No assignable types for manager interface [" + managerType.Name + "] found!");
            Assert.IsTrue<InvalidOperationException>(allAssignableTypes.Count > 1, "Manager interface [" + managerType.Name + "] cannot have more than one class! Has: " + allAssignableTypes.Count);

            Type foundInterfaceClassType = allAssignableTypes[0];
            BaseManager manager = managerByType[foundInterfaceClassType];
            return manager;
        }
        
        public TManagerInterface GetManager<TManagerInterface>(Type managerType) where TManagerInterface : class, IManager {
            TManagerInterface manager = (TManagerInterface)GetManager(managerType);
            return manager;
        }

        private void OnDestroy() {
            created = false;
        }

    }
}