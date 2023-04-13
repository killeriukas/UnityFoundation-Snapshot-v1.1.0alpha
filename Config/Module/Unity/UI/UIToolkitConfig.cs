using UnityEngine;
using UnityEngine.UIElements;
using UIController = TMI.UI.UIToolkit.UIController;

namespace TMI.ConfigManagement.Unity.UI.UIToolkit {
    
    [CreateAssetMenu(fileName = "UIToolkitConfig", menuName = "TMI/Config/Module/UI/UIToolkit", order = 0)]
    public class UIToolkitConfig : BaseUIConfig<UIController> {

        [SerializeField]
        private PanelSettings _panelSettings;

        public PanelSettings panelSettings => _panelSettings;

    }
    
}

