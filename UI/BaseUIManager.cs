using TMI.Core;
using TMI.AssetManagement;
using System.Collections.Generic;
using System;
using TMI.ConfigManagement.Unity.UI;
using UnityEngine;
using TMI.LogManagement;

namespace TMI.UI {

    [Loggable]
	public abstract class BaseUIManager : BaseManager, IUIManager {

        private enum State {
            Running,
            Precaching,
            Unknown
        }

        private State state = State.Unknown;

        private class UIMapping {
            public string prefabPath;
            public BaseUIController controller;
        }
        
        //    private readonly Dictionary<string, int> sortOrderNumberByLayerName = new Dictionary<string, int>();

		private readonly Dictionary<Type, UIMapping> cachedUIByType = new Dictionary<Type, UIMapping>();
        private readonly Dictionary<string, Type> cachedUiTypeByName = new Dictionary<string, Type>();


        
        protected override void Awake() {
            base.Awake();
            
            //foreach(SortingLayer sortingLayer in SortingLayer.layers) {
            //    sortOrderNumberByLayerName.Add(sortingLayer.name, 0);
            //}
            
		}

        public TUIController Load<TUIController>(bool refresh = true) where TUIController : BaseUIController {
            Assert.IsFalse<InvalidOperationException>(state == State.Running, "Cannot load the UI whilst UIManager is not running! Try BaseUIController.OnUIReady() instead. Current state: " + state);

            Type type = typeof(TUIController);
            
            Logging.Log(this, $"Loading UI for [{type.Name}].");
            
			UIMapping mapping = cachedUIByType[type];
            BaseUIController controller = mapping.controller;

            if(refresh) {
                controller.Refresh();
            }

			return (TUIController)controller;
		}

        protected abstract Transform GetUIParentTransform();
        
        private static GameObject CreateUIPrefab(GameObject uiPrefab, Transform parentTransform) {
            GameObject uiGameObject = Instantiate(uiPrefab, parentTransform, false);
            uiGameObject.name = uiGameObject.name.Replace("(Clone)", "");
            return uiGameObject;
        }
        
        public void Precache(ILoaderComplete loadedAssets) {
            state = State.Precaching;

            IEnumerable<string> uiPrefabPaths = loadedAssets.GetAll();
            foreach(string prefabPath in uiPrefabPaths) {
                GameObject loadedPrefab = loadedAssets.Get<GameObject>(prefabPath);

                GameObject uiGameObject = CreateUIPrefab(loadedPrefab, GetUIParentTransform());
                PrecacheUIPrefab(uiGameObject, prefabPath);
            }

            state = State.Running;

            foreach(var kvp in cachedUIByType) {
                BaseUIController baseUIController = kvp.Value.controller;
                baseUIController.OnUIReady();
            }
            
        }

        private void PrecacheUIPrefab(GameObject uiGameObject, string assetPath) {
            BaseUIController baseUIController = uiGameObject.GetComponent<BaseUIController>();
            
            Type controllerType = baseUIController.GetType();
            Logging.Log(this, "Precaching UI controller for [{0}].", controllerType.Name);
            
            baseUIController.Setup(initializer);
            baseUIController.Hide();

            
            UIMapping uiMapping = CreateUIMapping(assetPath, baseUIController);

            cachedUIByType.Add(controllerType, uiMapping);
            cachedUiTypeByName.Add(assetPath, controllerType);
        }

        private static UIMapping CreateUIMapping(string prefabPath, BaseUIController baseUIController) {
            UIMapping uiMapping = new UIMapping();
            uiMapping.prefabPath = prefabPath;
            uiMapping.controller = baseUIController;
            return uiMapping;
        }

        public void Unload(string prefabName) {
            Type uiType = cachedUiTypeByName[prefabName];
            
            Logging.Log(this, "Unloading UI controller for [{0}] for asset [{1}].", uiType.Name, prefabName);
            
            UIMapping uiMapping = cachedUIByType[uiType];
            CloseAndDestroy(uiMapping.controller);
            cachedUiTypeByName.Remove(prefabName);
            
            //TODO: make sure to unload this one from the asset manager as well
        }

        public abstract IUIConfig GetConfig();

        private void CloseAndDestroy(BaseUIController controller) {
			controller.Hide();
            
            Type type = controller.GetType();
            UIMapping mapping = cachedUIByType[type];
            cachedUIByType.Remove(type);

            Destroy(mapping.controller.gameObject);
        }

        protected override void OnDestroy() {
            
            foreach(var kvp in cachedUIByType) {
                UIMapping mapping = kvp.Value;
                Destroy(mapping.controller.gameObject);
            }
            cachedUIByType.Clear();
            cachedUiTypeByName.Clear();
            
            state = State.Unknown;
            base.OnDestroy();
        }

        //public int CreateSortingOrder(string sortingLayerName, int relativeDifference = 0) {
        //    Assert.IsTrue<ArgumentException>(relativeDifference < 0, "Relative difference cannot be < 0! Sorting layer: " + sortingLayerName + " Diff: " + relativeDifference);

        //    int currentSortingOrder = sortOrderNumberByLayerName[sortingLayerName];

        //    if(0 == relativeDifference) {
        //        ++currentSortingOrder;
        //    } else {
        //        currentSortingOrder += relativeDifference;
        //    }

        //    sortOrderNumberByLayerName[sortingLayerName] = currentSortingOrder;
        //    return currentSortingOrder;
        //}
    }


}