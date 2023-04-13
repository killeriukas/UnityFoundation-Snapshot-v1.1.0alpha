using System.Collections.Generic;
using UnityEngine;

namespace TMI.AssetManagement {

    public class ResourceGroup : IGroup {

        private readonly Dictionary<string, System.Type> assetTypeByAssetId = new Dictionary<string, System.Type>();

        public void Add<TAssetType>(string path) where TAssetType : Object {
            assetTypeByAssetId.Add(path, typeof(TAssetType));
        }

        public void AddGameObject(string path) {
            Add<GameObject>(path);
        }

        public ILoader CreateLoader() {
            ResourceLoader loader = new ResourceLoader(assetTypeByAssetId);
            return loader;
        }
        
    }

}