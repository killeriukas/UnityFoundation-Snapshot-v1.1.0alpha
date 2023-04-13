using UnityEngine;

namespace TMI.Core {

	public abstract class UnityBehaviour : MonoBehaviour {

		protected IInitializer initializer { get; private set; }
        public string sceneName { get; private set; }

        protected virtual void Awake() {
            sceneName = gameObject.scene.name;
        }

        protected virtual void Start() {

        }

		public virtual void Setup(IInitializer initializer) {
			this.initializer = initializer;
		}

		protected virtual void OnDestroy() {
			
		}

	}


}