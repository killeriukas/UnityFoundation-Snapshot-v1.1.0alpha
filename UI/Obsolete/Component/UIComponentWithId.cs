using UnityEngine;

namespace TMI.UI {

    public abstract class UIComponentWithId : UIComponent, IUIComponentWithId {

        [SerializeField]
        private string componentId;

        public string id { get { return componentId; } }
    }

    public abstract class UIComponentWithId<TComponentType> : UIComponent<TComponentType>, IUIComponentWithId where TComponentType : Component {

		[SerializeField]
		private string componentId;

		public string id { get { return componentId; } }
	}

}