using TMI.UI;
using UnityEngine;

namespace TMI.ConfigManagement.Unity.UI {
    
    [CreateAssetMenu(fileName = "UIConfig", menuName = "TMI/Config/Module/UI/uGUI", order = 0)]
    public class UIConfig : BaseUIConfig<UIController> {

        [SerializeField]
        private string _defaultUICameraTag = "UICamera";
        
        [SerializeField]
        private string _defaultUICanvasTag = "UICanvas";

        public string defaultUICameraTag => _defaultUICameraTag;
        public string defaultUICanvasTag => _defaultUICanvasTag;

    }
    
}

