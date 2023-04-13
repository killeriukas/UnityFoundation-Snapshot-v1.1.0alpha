using System.Collections.Generic;

namespace TMI.AssetManagement {

    public class SceneGroup : IGroup {

        private List<string> sceneNameList = new List<string>();

        public ILoader CreateLoader() {
            SceneLoader loader = new SceneLoader(sceneNameList);
            return loader;
        }

        public void Add(string sceneName) {
            sceneNameList.Add(sceneName);
        }
    }

}