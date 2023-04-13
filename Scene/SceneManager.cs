using TMI.Core;
using UnityEngine.SceneManagement;
using UnityEngine;
using System;
using TMI.AssetManagement;
using UnityEngine.EventSystems;

namespace TMI.SceneManagement {

    public class SceneManager : BaseManager, ISceneManager {

	    private IAssetManager assetManager;
	    
        private void OnSceneLoaded(Scene newScene) {
            Scene oldScene = UnityEngine.SceneManagement.SceneManager.GetActiveScene();

            GameObject[] newRootGameObjects = newScene.GetRootGameObjects();
            FindAndDeleteEventSystem(newRootGameObjects);

            GameObject[] rootGameObjects = oldScene.GetRootGameObjects();
            BaseInitializer oldInitializer = FindInitializer(rootGameObjects);
            oldInitializer.OnPreDestroy();
            
            CoreManager coreManager = FindCoreManager(rootGameObjects);

            UnityEngine.SceneManagement.SceneManager.MoveGameObjectToScene(coreManager.gameObject, newScene);
            UnityEngine.SceneManagement.SceneManager.SetActiveScene(newScene);
            UnityEngine.SceneManagement.SceneManager.UnloadSceneAsync(oldScene);
        }

        private static CoreManager FindCoreManager(GameObject[] gameObjectsArray) {
            foreach(GameObject go in gameObjectsArray) {
                CoreManager coreManager = go.GetComponent<CoreManager>();
                if(coreManager != null) {
                    return coreManager;
                }
            }

            throw new InvalidOperationException("CoreManager was not found! Something went horribly wrong!");
        }

        private static BaseInitializer FindInitializer(GameObject[] gameObjectsArray) {
	        foreach(GameObject go in gameObjectsArray) {
		        BaseInitializer baseInitializer = go.GetComponent<BaseInitializer>();
		        if(baseInitializer != null) {
			        return baseInitializer;
		        }
	        }

	        throw new InvalidOperationException("BaseInitializer was not found! Make sure every scene has one!");
        }
        
        private static void FindAndDeleteEventSystem(GameObject[] gameObjectsArray) {
	        foreach(GameObject go in gameObjectsArray) {
		        EventSystem eventSystem = go.GetComponent<EventSystem>();
		        if(eventSystem != null) {
			        GameObject.Destroy(eventSystem.gameObject);
		        }
	        }
        }

        public override void Setup(IInitializer initializer, bool isNew) {
	        base.Setup(initializer, isNew);
	        assetManager = initializer.GetManager<AssetManager, IAssetManager>();
        }

        public IHandle LoadAsync(string sceneName) {
	        SceneGroup sceneGroup = new SceneGroup();
	        sceneGroup.Add(sceneName);

	        Action<ILoaderComplete> onSceneLoadedClosure = (ILoaderComplete loadedAsset) =>
	        {
		        Scene newScene = loadedAsset.Get<Scene>(sceneName);
				OnSceneLoaded(newScene);
	        };

	        IRequestHandler requestHandler = RequestHandler.Create(onSceneLoadedClosure);
	        IHandle handle = assetManager.LoadAsync(sceneGroup, requestHandler);
	        return handle;
        }

	}


}