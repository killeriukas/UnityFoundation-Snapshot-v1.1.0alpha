using System;
using System.Collections.Generic;
using TMI.Helper;
using TMI.LogManagement;
using TMI.LogManagement.Unity;
using UnityEngine;

namespace TMI.ConfigManagement.Unity.Log {
    
    [CreateAssetMenu(fileName = "LoggingConfig", menuName = "TMI/Config/Module/Log/LogSettings", order = 0)]
    public class LoggingConfig : BaseConfig {

        [Serializable]
        private struct LogCategorySettings : IComparable<LogCategorySettings> {
            
            public string categoryName;
            public bool enabled;

            public int CompareTo(LogCategorySettings other) {
                return categoryName.CompareTo(other.categoryName);
            }
        }

        [SerializeField]
        private List<BaseLogOutput> logOutputList;
        
        [SerializeField]
        private List<LogCategorySettings> logSettingsList;

        [NonSerialized]
        private Dictionary<string, bool> logByCategory;

        protected override void OnInitialize_Runtime() {
            logByCategory = new Dictionary<string, bool>();

            foreach(LogCategorySettings logCategorySettings in logSettingsList) {
                logByCategory.Add(logCategorySettings.categoryName, logCategorySettings.enabled);
            }
            
            LogManagement.Logging.AddLogOutput(logOutputList);
        }

        public bool TryGetValue(string key, out bool value) {
            return logByCategory.TryGetValue(key, out value);
        }
        
        private static List<LogCategorySettings> CollectAllLogCategories() {
            List<Type> foundTypes = ReflectionHelper.FindAllClassesWithAttribute<LoggableAttribute>();
            
            List<LogCategorySettings> logCategories = foundTypes.ConvertAll(x => new LogCategorySettings() {categoryName = x.FullName, enabled = false});

            return logCategories;
        }
        
        
        private void Reset() {
            RefreshLogSettingsList();
            
            Debug.Log("Reset");
        }

        //may need to call this on a button click from the UI to refresh the asset
        private void RefreshLogSettingsList() {
            logSettingsList = CollectAllLogCategories();
            logSettingsList.Sort();
        }
        

    }
    
}

