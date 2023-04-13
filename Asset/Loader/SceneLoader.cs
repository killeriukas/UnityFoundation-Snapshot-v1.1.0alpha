using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace TMI.AssetManagement {

	public class SceneLoader : BaseLoader {

		private List<string> sceneNameList;

		private Dictionary<string, AsyncOperation> loadedSceneByName = new Dictionary<string, AsyncOperation>();

		public override float progress {
			get {
				int totalLoadPercentage = 100 * loadedSceneByName.Count;

				float totalLoadedPercentage = 0f;
				foreach(AsyncOperation asyncOperation in loadedSceneByName.Values) {
					totalLoadedPercentage += asyncOperation.progress;
				}

				float avgLoadedPercentage = totalLoadedPercentage / (float)totalLoadPercentage;
				return avgLoadedPercentage;
			}
		}

		public override bool isComplete {
			get {
				foreach(AsyncOperation asyncOperation in loadedSceneByName.Values) {
					if(!asyncOperation.isDone) {
						return false;
					}
				}
				return true;
			}
		}
		
		public override bool isEmpty => true;

		public SceneLoader(IEnumerable<string> sceneNames) {
			sceneNameList = new List<string>(sceneNames);
		}
		
		public override LoadHandle LoadAsync(IRequestExecutor executor) {
			LoadHandle loadHandle = new LoadHandle(this, executor);

			foreach(string sceneName in sceneNameList) {
				AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
				loadedSceneByName.Add(sceneName, asyncOperation);
			}
			
			return loadHandle;
		}

		//no cast test here, so if you ask for something which doesn't exist - expect the crash
		public override TObject Get<TObject>(string id) {
			
			AsyncOperation asyncOperation = loadedSceneByName[id];
			Assert.IsFalse<System.InvalidOperationException>(asyncOperation.isDone, "Cannot receive the scene! It's still loading. Progress: " + progress);

			Scene loadedScene = SceneManager.GetSceneByName(id);
			object castWorkaroundObject = loadedScene;
			
			return (TObject)castWorkaroundObject;
		}

		public override IEnumerable<string> GetAll() => loadedSceneByName.Keys;

		public override void UnloadAll() => throw new System.InvalidOperationException("Scene unloading is handled in the UnityEngine.SceneManager itself. If canceled - what did you expect to happen?");
		public override bool Unload<TObject>(TObject unloadObject) => throw new System.InvalidOperationException("Scene unloading is handled in the UnityEngine.SceneManager itself");
	}

}