using System;
using TMI.Helper;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace TMI.UI {

    [RequireComponent(typeof(TMP_InputField))]
    public class UIInputFieldPro : UIComponentWithId<TMP_InputField> {

        [SerializeField]
        private bool clearTextOnSubmit = false;

        public event Action<string> onSubmit;

        public string text {
            get {
                return component.text;
            }
            set {
                component.text = value;
            }
        }

        protected override void OnInitialize() {
            base.OnInitialize();
            component.onSubmit.AddListener(OnSubmit);
        }

        private void OnSubmit(string message) {
            onSubmit?.Invoke(message);
            if(clearTextOnSubmit) {
                component.text = "";
            }
        }

        protected override void OnDestroy() {
            component.onSubmit.RemoveAllListeners();
            base.OnDestroy();
        }

    }

}