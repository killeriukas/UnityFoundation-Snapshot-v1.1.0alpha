using System;
using System.Collections.Generic;
using UnityEngine;

namespace TMI.ConfigManagement.Unity {
    
    [CreateAssetMenu(fileName = "DefaultConfig", menuName = "TMI/Config/Default", order = 0)]
    public class DefaultConfig : ScriptableObject {
        
        [SerializeField]
        private List<BaseConfig> configModules;

        [NonSerialized]
        private Dictionary<Type, BaseConfig> configByType;

        [NonSerialized]
        private bool isInitialized = false;
        
        public TConfig GetConfig<TConfig>() where TConfig : BaseConfig {
            Type configType = typeof(TConfig);
            return (TConfig)configByType[configType];
        }

        public void Initialize_Runtime() {
            Assert.IsTrue<InvalidOperationException>(isInitialized, "Cannot initialize the default config again!");
            
            configByType = new Dictionary<Type, BaseConfig>();

            foreach(BaseConfig configModule in configModules) {
                configModule.Initialize_Runtime();
                configByType.Add(configModule.GetType(), configModule);
            }

            isInitialized = true;
        }
    }
}


