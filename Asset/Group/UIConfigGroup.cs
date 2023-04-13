using System.Collections.Generic;
using TMI.ConfigManagement.Unity.UI;
using TMI.LogManagement;

namespace TMI.AssetManagement {

    [Loggable]
    public class UIConfigGroup : IGroup {

        private List<string> prefabIds = new List<string>();
        private IUIConfig uiConfig;
        
        public UIConfigGroup(IUIConfig uiConfig) {
            this.uiConfig = uiConfig;
            prefabIds.AddRange(uiConfig.GetAllPermanentAssetIds_Once());
        }

        public void Add(string id) {
            if(prefabIds.Contains(id)) {
                Logging.LogWarning(this, "Adding the same asset id into UIConfigGroup. Ignoring. Id: " + id);
                return;
            }
            
            prefabIds.Add(id);
        }
        
        public ILoader CreateLoader() {
            UIConfigLoader loader = new UIConfigLoader(prefabIds, uiConfig);
            return loader;
        }
        
    }

}