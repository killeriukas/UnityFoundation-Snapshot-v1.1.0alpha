using System;
using UnityEngine;
using UnityEngine.UI;

namespace TMI.UI {

    [RequireComponent(typeof(Toggle))]
    public class UIToggle : UIComponentWithId<Toggle> {

        public event Action<bool> onValueChanged;

        protected override void OnInitialize() {
            base.OnInitialize();
            component.onValueChanged.AddListener(OnToggleValueChanged);
        }

        public bool isOn {
            get {
                return component.isOn;
            }
            set {
                component.isOn = value;
            }
        }

        private void OnToggleValueChanged(bool currentValue) {
            onValueChanged?.Invoke(currentValue);
        }

        protected override void OnDestroy() {
            component.onValueChanged.RemoveAllListeners();
            base.OnDestroy();
        }

    }

}