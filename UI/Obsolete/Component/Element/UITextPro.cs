using TMPro;
using UnityEngine;

namespace TMI.UI {

    [RequireComponent(typeof(TextMeshProUGUI))]
    public class UITextPro : UIComponentWithId<TextMeshProUGUI> {

        public string text {
			get { 
				return component.text;
			}
			set {
				component.text = value;
			}
		}

        public Color color {
            set {
                component.color = value; 
            }
        }
    }

}