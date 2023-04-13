using System.Collections.Generic;
using UnityEngine;

namespace TMI.ConfigManagement.Unity.UI {
    
    public interface IUIConfig {
        IEnumerable<string> GetAllPermanentAssetIds_Once();
        public GameObject GetPrefab(string id);
    }
    
}

