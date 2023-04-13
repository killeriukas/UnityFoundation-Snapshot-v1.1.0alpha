using TMI.ConfigManagement.Unity;
using TMI.ConfigManagement.Unity.UI;
using TMI.ConfigManagement.Unity.UI.UIToolkit;
using UnityEngine;
using TMI.LogManagement;

namespace TMI.UI.UIToolkit {

    [Loggable]
	public class UIManager : BaseUIManager, IUIManager {
        protected override Transform GetUIParentTransform() => transform;
        public override IUIConfig GetConfig() => ConfigManager.GetConfig<UIToolkitConfig>();
    }


}