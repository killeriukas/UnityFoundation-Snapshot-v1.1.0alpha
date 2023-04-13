using TMI.Core;

namespace TMI.AssetManagement {

    public interface IAssetManager : IManager {
        IHandle LoadAsync(IGroup group, IRequestJoiner requestJoiner);
        bool Unload<TObject>(TObject unloadObject);
    }


}

