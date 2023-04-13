using TMI.UI.Extension;

namespace TMI.UI {

    public class UIProgressBar : UIComponentWithId<ProgressBar> {

        public float fillAmount {
			set {
				component.fillAmount = value;
			}
		}

		public string text {
			set { 
				component.text = value;
			}
		}

        public bool enableText {
            set {
                component.enableText = value;
            }
        }

        public void SetAlternativeText(string id, string text) {
            component.SetAlternativeText(id, text);
        }

    }
}

