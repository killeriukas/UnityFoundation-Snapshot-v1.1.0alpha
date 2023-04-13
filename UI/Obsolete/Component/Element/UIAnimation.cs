using UnityEngine;

namespace TMI.UI {

    [RequireComponent(typeof(Animation))]
    public class UIAnimation : UIComponentWithId<Animation> {

        [SerializeField]
        private bool playOnShow = false;

        protected override void OnInitialize() {
            base.OnInitialize();
            component.playAutomatically = false;
        }

        public bool playAnimationOnShow {
            get {
                return playOnShow;
            }
        }

        public void Play() {
            component.Play();
        }

    }

}