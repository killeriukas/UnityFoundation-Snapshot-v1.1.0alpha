using TMI.LogManagement;

namespace TMI.ConfigManagement.Unity {

    [Loggable(isMandatory = true)]
    public static class ConfigManager {

        private static DefaultConfig defaultConfig;
        
        public static void Load(DefaultConfig defaultConfig) {
            if(ConfigManager.defaultConfig != null) {
                Logging.Log(typeof(ConfigManager).FullName, "Ignoring the override of already set default config!");
                return;
            }

            defaultConfig.Initialize_Runtime();
            ConfigManager.defaultConfig = defaultConfig;
        }
        
        public static TConfig GetConfig<TConfig>() where TConfig : BaseConfig {
            return defaultConfig.GetConfig<TConfig>();
        }

        public static void Clear() {
            defaultConfig = null;
        }
        
    }


}