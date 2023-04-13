using TMI.AssetManagement;
using TMI.Core;

namespace TMI.SceneManagement {

    public interface ISceneManager : IManager {
        IHandle LoadAsync(string sceneName);
    }
}