using System;
using System.Collections.Generic;
using UnityEngine;
using UnityObject = UnityEngine.Object;

namespace TMI.AssetManagement {

	public class ResourceLoader : BaseLoader {

		private Dictionary<string, Type> assetTypeByAssetId;
		
		private Dictionary<string, ResourceRequest> loadRequestByPath = new Dictionary<string, ResourceRequest>();
		
		public ResourceLoader(IEnumerable<KeyValuePair<string, Type>> assetTypeByPath) {
			assetTypeByAssetId = new Dictionary<string, Type>(assetTypeByPath);
		}

		public override float progress {
			get {
				int totalLoadPercentage = 100 * loadRequestByPath.Count;

				float totalLoadedPercentage = 0f;
				foreach(ResourceRequest resourceRequest in loadRequestByPath.Values) {
					totalLoadedPercentage += resourceRequest.progress;
				}

				float avgLoadedPercentage = totalLoadedPercentage / (float)totalLoadPercentage;
				return avgLoadedPercentage;
			}
		}

		public override bool isComplete {
			get {
				foreach(ResourceRequest resourceRequest in loadRequestByPath.Values) {
					if(!resourceRequest.isDone) {
						return false;
					}
				}
				return true;
			}
		}
		public override bool isEmpty => loadRequestByPath.Count == 0;

		public override void UnloadAll() {
			foreach(ResourceRequest resourceRequest in loadRequestByPath.Values) {
				Resources.UnloadAsset(resourceRequest.asset);
			}
			
			loadRequestByPath.Clear();
		}
		
		public override LoadHandle LoadAsync(IRequestExecutor executor) {
			LoadHandle loadHandle = new LoadHandle(this, executor);
			
			foreach(var kvp in assetTypeByAssetId) {
				ResourceRequest resourceRequest = Resources.LoadAsync(kvp.Key, kvp.Value);
				loadRequestByPath.Add(kvp.Key, resourceRequest);
			}
			
			return loadHandle;
		}
		
		public override bool Unload<TObject>(TObject unloadObject) {
			UnityObject unityObject = unloadObject as UnityObject;
			
			if(unityObject == null) {
				throw new InvalidCastException("Requesting to unload TObject type which is not of UnityEngine.Object!");
			}

			var keys = loadRequestByPath.Keys;
			foreach(string path in keys) {
				ResourceRequest resourceRequest = loadRequestByPath[path];
				if(unityObject == resourceRequest.asset) {
					Resources.UnloadAsset(unityObject);
					loadRequestByPath.Remove(path);
					return true;
				}
			}

			return false;
		}
		
		//no cast test here, so if you ask for something which doesn't exist - expect the crash
		public override TObject Get<TObject>(string id) {
			ResourceRequest resourceRequest = loadRequestByPath[id];
			object castWorkaroundObject = resourceRequest.asset;
			return (TObject)castWorkaroundObject;
		}
		
		public override IEnumerable<string> GetAll() {
			return loadRequestByPath.Keys;
		}

	}

}