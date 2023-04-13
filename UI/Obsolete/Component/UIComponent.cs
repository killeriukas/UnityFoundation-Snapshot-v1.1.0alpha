using UnityEngine.EventSystems;
using UnityEngine;
using TMI.Core;
using TMI.LogManagement;
using System;
using TMI.Helper;

namespace TMI.UI {

    [Loggable]
	public abstract class UIComponent : UIBehaviour, IUIComponent {

        private enum State {
            Unknown,
            Initialized
        }

        protected string sceneName { get; private set; }

        private State state = State.Unknown;

        protected bool isSetup { get; private set; }
        protected IInitializer initializer { get; private set; }

        public void OnCloneComplete() {
            if(state != State.Initialized) {
                OnInitialize();
            }
        }

        /// <summary>
        /// Called on active and inactive objects, once they are created
        /// Awake is called only on active objects, hence it's sealed and cannot be used
        /// </summary>
        protected virtual void OnInitialize() {
            sceneName = gameObject.scene.name;
            Type currentType = GetType();
            Logging.Log(this, "Initializing UI element in scene [{0}] on gameObject [{1}] called [{2}].", sceneName, name, currentType.Name);
            state = State.Initialized;
        }

        sealed protected override void Awake() {
            base.Awake();

            if(state != State.Initialized) {
                OnInitialize();
            }
        }

        public virtual void Setup(IInitializer initializer) {
            Assert.IsTrue<InvalidOperationException>(isSetup, "Calling Setup() on already setup component: " + GeneralHelper.GenerateTransformHierarchyString(transform));

            this.initializer = initializer;

            //cannot set this, because animations will lose connections to the game objects
            //some smart ass made animations hook up to the game objects using NAMES... WTF?! Are you out of your mind?!
//            name = name + "_[Setup]";

            isSetup = true;
        }
    }

	public abstract class UIComponent<TComponentType> : UIComponent where TComponentType : Component {

        protected TComponentType component { get; private set; }

        protected override void OnInitialize() {
            base.OnInitialize();
            component = GetComponent<TComponentType>();
            Assert.IsNull(component, "UIComponent<TComponentType> cannot be set on gameObject without TComponentType! GameObject: " + GeneralHelper.GenerateTransformHierarchyString(transform));
        }

	}

}