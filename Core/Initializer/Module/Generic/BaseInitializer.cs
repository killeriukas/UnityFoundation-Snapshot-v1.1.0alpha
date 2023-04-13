using System;
using UnityEngine;

namespace TMI.Core {

    public abstract class BaseInitializer : MonoBehaviour, IInitializer {
        
        private CoreManager coreManager;

        protected virtual void PreAwake() {
        }
        
		protected virtual void Awake() {
            PreAwake();
            coreManager = CoreManager.Create();

            IAcquirer acquirer = coreManager.StartAcquire();
			RegisterManagers(acquirer);
            coreManager.EndAcquire(acquirer, this);
		}

        protected abstract void RegisterManagers(IAcquirer acquirer);

        public TManagerInterface GetManager<TManager, TManagerInterface>() where TManager : BaseManager, TManagerInterface, new() where TManagerInterface : class, IManager {
            return coreManager.GetManager<TManager, TManagerInterface>();
        }

        public TManagerInterface GetManager<TManagerInterface>(Type managerType) where TManagerInterface : class, IManager {
            return coreManager.GetManager<TManagerInterface>(managerType);
        }

        public virtual void OnPreDestroy() {
        }

        protected virtual void OnDestroy() {
        }

    }

}