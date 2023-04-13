using System.Collections.Generic;
using TMI.Core;
using TMI.Helper;
using TMI.UI.Extension;
using UnityEngine.EventSystems;

namespace TMI.UI {

	public abstract class UIController : BaseUIController {

        private UIAnimation[] uiAnimations;

        protected IUIManager uiManager { get; private set; }

        protected override void Awake() {
            base.Awake();
            uiAnimations = GetComponentsInChildren<UIAnimation>(true);
        }

        //protected override void Awake() {
        //    base.Awake();
        //    mainCanvas = GetComponent<Canvas>();
        //    Assert.IsFalse<ArgumentException>(mainCanvas.renderMode == RenderMode.ScreenSpaceCamera, "Camera render mode must be set to ScreenSpaceCamera! Scene: " + sceneName);

        //    subCanvases = this.GetComponentsInChildrenOnly<Canvas>(true);
        //}

        public override void Setup(IInitializer initializer) {
	        base.Setup(initializer);
	        uiManager = initializer.GetManager<UIManager, IUIManager>();

	        //initialize all components excluding cells and their children
	        //cells are initialized when they are cloned
	        IEnumerable<IUIComponent> uiComponentsWithoutCells = HierarchyHelper.GetComponentsInChildrenOnlyExcludingOne<IUIComponent, Cell>(this);
	        foreach(IUIComponent component in uiComponentsWithoutCells) {
		        component.Setup(initializer);
	        }
        }

        public override bool Show() {
	        bool isSuccess = base.Show();

	        if(isSuccess) {
		        gameObject.SetActive(true);

		        foreach(UIAnimation uiAnimation in uiAnimations) {
			        if(uiAnimation.playAnimationOnShow) {
				        uiAnimation.Play();
			        }
		        }
	        }

            return isSuccess;
        }

		public override bool Hide() {
			bool isSuccess = base.Hide();

			if(isSuccess) {
				gameObject.SetActive(false);	
			}

			return isSuccess;
		}

		protected void Close(PointerEventData eventData) {
			Hide();
		}

	}

}