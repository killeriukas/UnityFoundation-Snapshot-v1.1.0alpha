using TMI.Core;
using UnityEngine;
using UnityEngine.UIElements;

namespace TMI.UI.UIToolkit {

    [RequireComponent(typeof(UIDocument))]
    public abstract class UIController : BaseUIController {

        private UIDocument uiDocument;
        protected VisualElement rootVisualElement => uiDocument.rootVisualElement;

        public override void Setup(IInitializer initializer) {
            base.Setup(initializer);
            uiDocument = GetComponent<UIDocument>();
        }

        public override bool Show() {
            bool isSuccess = base.Show();

            if(isSuccess) {
                rootVisualElement.style.display = DisplayStyle.Flex;    
            }

            return isSuccess;
        }
        
        public override bool Hide() {
            bool isSuccess = base.Hide();

            if(isSuccess) {
                rootVisualElement.style.display = DisplayStyle.None;    
            }

            return isSuccess;
        }

    }

}