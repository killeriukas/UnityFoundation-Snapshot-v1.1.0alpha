using TMI.UI.Extension;
using System;
using UnityEngine.EventSystems;
using UnityEngine;
using TMI.Core;
using TMI.Helper;

namespace TMI.UI {

	public class UIButton : UIComponentWithId<Button>, IUpdatable {

        private enum ButtonClickState {
            Click,
            RepeatClick
        }

        public enum ButtonState {
            Repeat,
            Stop
        }

        [SerializeField]
        private bool enableLongPressRepeatClick = false;

        private TimeSpan minLongPressDuration { get { return TimeSpan.FromMilliseconds(50); } }
        private TimeSpan maxLongPressDuration { get { return TimeSpan.FromMilliseconds(500); } }

        [SerializeField]
        private UITextPro mainText;

        private IExecutionManager executionManager;
        private ButtonClickState buttonClickState = ButtonClickState.Click;

        private bool isButtonDown = false;
        private TimeSpan currentPressDuration;
        private TimeSpan currentMaxLongPressDuration;

        public event Func<ButtonState> onRepeatLongPressClick;

        public override void Setup(IInitializer initializer) {
            base.Setup(initializer);
            executionManager = initializer.GetManager<ExecutionManager, IExecutionManager>();

            if(enableLongPressRepeatClick) {
                component.onButtonDown += OnButtonDown;
                component.onButtonUp += OnButtonUp;
                component.onButtonClick += OnButtonClick;

                executionManager.Register(this, OnUpdate);
            }
        }

        private void OnButtonClick(PointerEventData data) {
            if(buttonClickState == ButtonClickState.Click) {
                if(onRepeatLongPressClick != null) {
                    onRepeatLongPressClick();
                }
            }
        }

        private ExecutionManager.Result OnUpdate() {

            if(isButtonDown) {
                currentPressDuration += TimeSpan.FromSeconds(Time.deltaTime);

                if(currentPressDuration > currentMaxLongPressDuration) {
                    currentPressDuration = TimeSpan.Zero;
                    buttonClickState = ButtonClickState.RepeatClick;

                    double nextMaxLongPressDurationMSec = currentMaxLongPressDuration.TotalMilliseconds * 0.5;
                    
                    currentMaxLongPressDuration = TimeSpan.FromMilliseconds(Math.Clamp(nextMaxLongPressDurationMSec, minLongPressDuration.TotalMilliseconds, nextMaxLongPressDurationMSec));

                    if(onRepeatLongPressClick != null) {
                        ButtonState buttonState = onRepeatLongPressClick();
                        if(buttonState == ButtonState.Stop) {
                            //might be better off to finish the execution totally here - test it later
                            isButtonDown = false;
                        }
                    }
                }

            }

            return ExecutionManager.Result.Continue;
        }

        private void OnButtonDown(PointerEventData pointerEventData) {
            buttonClickState = ButtonClickState.Click;
            currentMaxLongPressDuration = maxLongPressDuration;
            currentPressDuration = TimeSpan.Zero;
            isButtonDown = true;
        }

        private void OnButtonUp(PointerEventData pointerEventData) {
            isButtonDown = false;
        }

        public event Action<PointerEventData> onButtonClick {
			add {
				component.onButtonClick += value;
			}
			remove {
				component.onButtonClick -= value;
			}
		}

        public bool interactable {
            get {
                return component.interactable;
            }
            set {
                component.interactable = value;
            }
        }

        public UnityEngine.UI.Selectable.Transition transition {
            get {
                return component.transition;
            }
            set {
                component.transition = value;
            }
        }

        public string text {
            set {
                mainText.text = value;
            }
        }

        public bool enableText {
            set {
                mainText.gameObject.SetActive(value);
            }
        }

        public void ClearOnButtonClick() {
            component.ClearOnButtonClick();
        }

        protected override void OnDestroy() {
            if(isSetup) {
                if(enableLongPressRepeatClick) {
                    component.onButtonDown -= OnButtonDown;
                    component.onButtonUp -= OnButtonUp;
                    component.onButtonClick -= OnButtonClick;

                    executionManager.Unregister(this);
                }
            }
            base.OnDestroy();
        }
    }

}

