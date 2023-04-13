using TMI.Core;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace TMI.AssetManagement {

	public class AssetManager : BaseManager, IAssetManager {

		private List<Coroutine> assetLoadingList = new List<Coroutine>();
		private List<ILoader> allLoaders = new List<ILoader>();

		public IHandle LoadAsync(IGroup resourceGroup, IRequestJoiner requestJoiner) {
			ILoader loader = resourceGroup.CreateLoader();
			LoadHandle handle = loader.LoadAsync(requestJoiner.GetExecutor());

			if(!loader.isEmpty) {
				allLoaders.Add(loader);	
			}

			StartCoroutine(RunCoroutine(loader, handle));
			return handle;
		}

		private IEnumerator RunCoroutine(ILoader loader, LoadHandle handle) {

			//never load on the first frame, because it's an async operation after all
			yield return null;

			//load until the loader is complete, or the handle has been canceled
			while(!loader.isComplete) { // && !handle.isCanceled
				//handle.SetProgress(loader);
				yield return null;
			}

			// if(handle.isCanceled) {
			// 	loader.UnloadAll();
			// } else {
			// 	handle.Complete(loader);
			// }

			if(handle.isValid) {
				handle.Complete();
			}
		}

		public bool Unload<TObject>(TObject unloadObject) {
			for(int i = allLoaders.Count - 1; i >= 0; --i) {
				ILoader loader = allLoaders[i];
				bool success = loader.Unload(unloadObject);
				if(success) {
					if(loader.isEmpty) {
						allLoaders.RemoveAt(i);
					}
					return true;
				}
			}

			return false;
		}

		protected override void OnDestroy() {
			foreach(Coroutine coroutine in assetLoadingList) {
				StopCoroutine(coroutine);
			}
			assetLoadingList.Clear();

			foreach(ILoader loader in allLoaders) {
				loader.UnloadAll();
			}
			
			allLoaders.Clear();

			base.OnDestroy();
		}
	}

}