using TMI.AssetManagement;
using TMI.ConfigManagement.Unity.UI;
using TMI.Core;

namespace TMI.UI {

    public interface IUIManager : IManager {
        TUIController Load<TUIController>(bool refresh = true) where TUIController : BaseUIController;

        void Precache(ILoaderComplete loadedAssets);

        void Unload(string id);
        
        //  int CreateSortingOrder(string sortingLayerName, int relativeDifference = 0);
        IUIConfig GetConfig();
    }

}