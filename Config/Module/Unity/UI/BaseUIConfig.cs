using System;
using System.Collections.Generic;
using TMI.LogManagement;
using TMI.UI;
using UnityEngine;

namespace TMI.ConfigManagement.Unity.UI {
    
    [Loggable(isMandatory = true)]
    public abstract class BaseUIConfig<TUIController> : BaseConfig, IUIConfig where TUIController : BaseUIController {

        [Serializable]
        private class UIAssetMap {
            public string label;
            public TUIController uiPrefab;
        }

        [SerializeField]
        private List<UIAssetMap> permanentUIAssetMap;
        
        [SerializeField]
        private List<UIAssetMap> localUIAssetMap;

        //FOR LATER
        //could have a useResourceFolder build step flag and copy all the ui files into the resources folder on the BUILD step, and delete after the build

        private List<UIAssetMap> allUIAssets = new List<UIAssetMap>();
        
        //what about the downloaded maps which aren't included in the build????
      //  private List<UIAssetMap> dynamicAssetsMap;

      [NonSerialized]
        private bool isPermanentUIAssetMapLoaded = false;

        protected override void OnInitialize_Runtime() {
            allUIAssets.AddRange(permanentUIAssetMap);
            allUIAssets.AddRange(localUIAssetMap);
        }

        private TUIController FindPrefabById(string id) {
            foreach(UIAssetMap uiAssetMap in allUIAssets) {
                if(uiAssetMap.label.Equals(id)) {
                    return uiAssetMap.uiPrefab;
                }
            }

            throw new ArgumentException($"Prefab id was not found inside the config file. Id: [{id}].");
        }

        //returns permanent asset ids only once
        //first time -> permanent asset ids
        //nth time -> empty list
        public IEnumerable<string> GetAllPermanentAssetIds_Once() {
            if(isPermanentUIAssetMapLoaded) {
                return new List<string>();
            }
            
            isPermanentUIAssetMapLoaded = true;
            return permanentUIAssetMap.ConvertAll(x => x.label);
        }
        
        public GameObject GetPrefab(string id) {
            GameObject foundGameObject = FindPrefabById(id).gameObject;
            
            if(!foundGameObject.activeSelf) {
                LogManagement.Logging.LogWarning(this, $"Loaded prefab [{foundGameObject.name}] is inactive. Make sure it's as intended.");    
            }
            
            return foundGameObject;
        }
        
    }
    
}

