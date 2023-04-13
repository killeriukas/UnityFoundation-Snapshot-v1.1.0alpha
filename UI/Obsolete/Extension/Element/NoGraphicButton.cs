using UnityEngine.EventSystems;
using System;

namespace TMI.UI.Extension {

    public class NoGraphicButton : BaseNoGraphic, IPointerClickHandler {

        public event Action<PointerEventData> onClick;

        #region IPointerClickHandler implementation
        public void OnPointerClick(PointerEventData eventData)
        {
            if(onClick != null) {
                onClick(eventData);
            }
        }
        #endregion

    }


}