using System;
using System.Collections.Generic;
using TMI.UI;
using UnityEngine;

namespace TMI.ConfigManagement.Unity.UI {
    
    [CreateAssetMenu(fileName = "UISettingsConfig", menuName = "TMI/Config/Module/UI/Settings", order = 0)]
    public class UISettingsConfig : BaseConfig {
        
        private enum UIManagerType {
            UI_Old = 0,
            UIToolkit,
        }

        [SerializeField]
        private UIManagerType defaultUIManagerType;

        public Type GetDefaultUIManagerType() {
            switch(defaultUIManagerType) {
                case UIManagerType.UI_Old:
                    return typeof(UIManager);
                case UIManagerType.UIToolkit:
                    return typeof(TMI.UI.UIToolkit.UIManager);
                default:
                    throw new ArgumentException("None default UI manager selected!");
            }
        }

        protected override void OnInitialize_Runtime() {
        }
    }
    
}

