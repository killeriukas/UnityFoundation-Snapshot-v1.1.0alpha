using UnityEngine;
using UnityEngine.UI;

namespace TMI.UI {

    [RequireComponent(typeof(Image))]
    public class UIImage : UIComponentWithId<Image> {

        private Material imageMaterial;

        protected override void OnInitialize() {
            base.OnInitialize();
            imageMaterial = component.material;
        }

        public Sprite sprite {
            get {
                return component.sprite;
            }
            set {
                component.sprite = value;
            }
        }

        public float fillAmount {
            get {
                return component.fillAmount;
            }
            set {
                component.fillAmount = value;
            }
        }

        public Color color {
            set {
                component.color = value;
            }
        }

        public void SetShaderFloat(string name, float value) {
            imageMaterial.SetFloat(name, value);
        }

    }
}