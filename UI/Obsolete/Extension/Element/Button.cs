using System;
using UnityEngine.EventSystems;

namespace TMI.UI.Extension {

    public class Button : UnityEngine.UI.Button {

        public event Action<PointerEventData> onButtonDown;
        public event Action<PointerEventData> onButtonUp;
        public event Action<PointerEventData> onButtonClick;

        public override void OnPointerUp(PointerEventData eventData) {
            base.OnPointerUp(eventData);

            if(interactable) {
                onButtonUp?.Invoke(eventData);
            }
        }

        public override void OnPointerDown(PointerEventData eventData) {
            base.OnPointerDown(eventData);

            if(interactable) {
                onButtonDown?.Invoke(eventData);
            }
        }

        public override void OnPointerClick(PointerEventData eventData) {
			base.OnPointerClick(eventData);

			if(interactable) {
                onButtonClick?.Invoke(eventData);
			}
        }

        public void ClearOnButtonClick() {
            onButtonDown = null;
            onButtonUp = null;
            onButtonClick = null;
        }

    }
        
}