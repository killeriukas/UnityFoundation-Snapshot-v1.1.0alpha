using System;
using System.Collections.Generic;
using TMI.ConfigManagement.Unity.UI;
using UnityEngine;
using UnityObject = UnityEngine.Object;

namespace TMI.AssetManagement {

    public class UIConfigLoader : BaseLoader {

        private IUIConfig uiConfig;
        
        private List<string> prefabIds;
        
        private Dictionary<string, GameObject> prefabById = new Dictionary<string, GameObject>();

        public override float progress => 1f;
        public override bool isComplete => true;

        public UIConfigLoader(IEnumerable<string> prefabIds, IUIConfig uiConfig) {
            this.prefabIds = new List<string>(prefabIds);
            this.uiConfig = uiConfig;
        }

        public override LoadHandle LoadAsync(IRequestExecutor executor) {

            foreach(string id in prefabIds) {
                GameObject prefab = uiConfig.GetPrefab(id);
                prefabById.Add(id, prefab);
            }
            
            LoadHandle loadHandle = new LoadHandle(this, executor);
            return loadHandle;
        }

        public override bool isEmpty => prefabById.Count == 0;

        public override void UnloadAll() {
            prefabById.Clear();
        }

        public override bool Unload<TObject>(TObject unloadObject) {
            UnityObject unityObject = unloadObject as UnityObject;
			
            if(unityObject == null) {
                throw new InvalidCastException("Requesting to unload TObject type which is not of UnityEngine.Object!");
            }
            
            var keys = prefabById.Keys;
            foreach(string path in keys) {
                GameObject prefab = prefabById[path];
                if(unityObject == prefab) {
                    prefabById.Remove(path);
                    return true;
                }
            }

            return false;    
        }

        public override TObject Get<TObject>(string id) {
            GameObject prefab = prefabById[id];
            object castWorkaroundObject = prefab;
            return (TObject)castWorkaroundObject;
        }

        public override IEnumerable<string> GetAll() => prefabById.Keys;
    }

}