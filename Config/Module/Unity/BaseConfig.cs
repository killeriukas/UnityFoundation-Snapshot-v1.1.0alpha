using System;
using UnityEngine;

namespace TMI.ConfigManagement.Unity {
    
    public abstract class BaseConfig : ScriptableObject, IConfig {
        
        [NonSerialized]
        private bool isInitialized = false;
        
        public void Initialize_Runtime() {
            Assert.IsTrue<InvalidOperationException>(isInitialized, $"Cannot initialize the {GetType().FullName} again!");
            OnInitialize_Runtime();
            isInitialized = true;
        }

        protected abstract void OnInitialize_Runtime();
    }

    
}

