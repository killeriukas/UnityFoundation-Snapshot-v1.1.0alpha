using System;
using TMI.UI.Extension;
using UnityEngine;
using UnityEngine.UI;

namespace TMI.UI {

    [RequireComponent(typeof(Slider))]
    public class UISlider : UIComponentWithId<Slider> {

        [Serializable]
        private class Mapping {
            public string id;
            public UITextPro text;
        }

        [SerializeField]
        private UITextPro mainText;

        [SerializeField]
        private Mapping[] alternativeText;

        public float value {
            get {
                return component.value;
            }
            set {
                component.value = value;
            }
        }

        public string text {
            set {
                mainText.text = value;
            }
        }

        public void SetAlternativeText(string id, string text) {
            foreach(Mapping mapping in alternativeText) {
                if(mapping.id.Equals(id)) {
                    mapping.text.text = text;
                    break;
                }
            }
        }

    }

}