using TMI.ConfigManagement.Unity;
using TMI.ConfigManagement.Unity.UI;
using UnityEngine;
using TMI.LogManagement;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace TMI.UI {

    [Loggable]
	public class UIManager : BaseUIManager, IUIManager {

        private EventSystem uiEventSystem;
        private Camera uiCamera;
        private Canvas uiCanvas;
        
        private static EventSystem CreateEventSystem() {
            GameObject newEventSystemGo = new GameObject("EventSystem");

            EventSystem eventSystem = newEventSystemGo.AddComponent<EventSystem>();
            eventSystem.sendNavigationEvents = true;
            eventSystem.pixelDragThreshold = 30;

            StandaloneInputModule standaloneInputModule = newEventSystemGo.AddComponent<StandaloneInputModule>();
            standaloneInputModule.horizontalAxis = "Horizontal";
            standaloneInputModule.verticalAxis = "Vertical";
            standaloneInputModule.submitButton = "Submit";
            standaloneInputModule.cancelButton = "Cancel";
            standaloneInputModule.inputActionsPerSecond = 10f;
            standaloneInputModule.repeatDelay = 0.5f;

            return eventSystem;
        }
        
        private static Canvas CreateCanvas(string tagName, Camera uiCamera, bool isVertical) {
            GameObject newCanvasGo = new GameObject("UI Canvas");
            newCanvasGo.tag = tagName;
            newCanvasGo.layer = LayerMask.NameToLayer("UI");

            Canvas newCanvas = newCanvasGo.AddComponent<Canvas>();
            newCanvas.renderMode = RenderMode.ScreenSpaceCamera;
            newCanvas.worldCamera = uiCamera;
            newCanvas.planeDistance = 100f;

            CanvasScaler canvasScaler = newCanvasGo.AddComponent<CanvasScaler>();
            canvasScaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            canvasScaler.referenceResolution = isVertical ? new Vector2(720, 1280) : new Vector2(1280, 720);
            canvasScaler.screenMatchMode = CanvasScaler.ScreenMatchMode.MatchWidthOrHeight;
            canvasScaler.matchWidthOrHeight = isVertical ? 1f : 0f;
            canvasScaler.referencePixelsPerUnit = 100f;

            GraphicRaycaster graphicRaycaster = newCanvasGo.AddComponent<GraphicRaycaster>();
            graphicRaycaster.ignoreReversedGraphics = true;
            graphicRaycaster.blockingObjects = GraphicRaycaster.BlockingObjects.None;

            return newCanvas;
        }
        
        private static Camera CreateUICamera(string tagName) {
            GameObject newCameraGo = new GameObject("UI Camera");
            newCameraGo.tag = tagName;
            Camera uiCamera = newCameraGo.AddComponent<Camera>();

            uiCamera.clearFlags = CameraClearFlags.Depth;
            uiCamera.cullingMask = 1 << LayerMask.NameToLayer("UI");
            uiCamera.orthographic = true;
            uiCamera.orthographicSize = 8f;
            uiCamera.nearClipPlane = 0.3f;
            uiCamera.farClipPlane = 110f;
            uiCamera.depth = 0f;
            uiCamera.useOcclusionCulling = false;
            return uiCamera;
        }
        
        protected override void Awake() {
            base.Awake();

            uiEventSystem = GameObject.FindObjectOfType<EventSystem>();
            
            //create the main ui event system
            if(uiEventSystem == null) {
                uiEventSystem = CreateEventSystem();
            }
            
            uiEventSystem.transform.SetParent(transform, false);
            
            const string uiCameraTag = "UICamera";
            GameObject foundCamera = GameObject.FindGameObjectWithTag(uiCameraTag);
            
            //create default UI camera
            if(foundCamera == null) {
                uiCamera = CreateUICamera(uiCameraTag);
            } else {
                uiCamera = foundCamera.GetComponent<Camera>();
            }
            
            uiCamera.transform.SetParent(transform, false);
            
            //create the main UI canvas
            const string uiCanvasTag = "UICanvas";
            GameObject foundCanvas = GameObject.FindGameObjectWithTag(uiCanvasTag);
            if(foundCanvas == null) {
                uiCanvas = CreateCanvas(uiCanvasTag, uiCamera, false);
            } else {
                uiCanvas = foundCanvas.GetComponent<Canvas>();
            }
            
            uiCanvas.transform.SetParent(transform, false);
        }

        protected override Transform GetUIParentTransform() => uiCanvas.transform;
        public override IUIConfig GetConfig() => ConfigManager.GetConfig<UIConfig>();
    }


}