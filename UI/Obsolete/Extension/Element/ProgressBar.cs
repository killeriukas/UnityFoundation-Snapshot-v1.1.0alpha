using UnityEngine;
using System;

namespace TMI.UI.Extension {

	public class ProgressBar : UIComponent {

		[Serializable]
		private class Mapping {
			public string id;
			public UITextPro text;
		}

		[SerializeField]
		private UnityEngine.UI.Image background;

		[SerializeField]
		private UnityEngine.UI.Image foreground;

		[SerializeField]
		private UITextPro mainText;

		[SerializeField]
		private Mapping[] alternativeText;

		public float fillAmount {
			set {
				foreground.fillAmount = value;
			}
		}

		public string text {
			set { 
				mainText.text = value;
			}
		}

        public bool enableText {
            set {
                mainText.gameObject.SetActive(value);
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