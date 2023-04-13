using System;
using TMI.LogManagement;
using TMI.Notification;
using UnityEngine;

namespace TMI.UI {

	[Loggable]
	public abstract class BaseUIController : BaseNotificationBehaviour {

        private enum State {
            Unknown,
            Showing,
            Hidden
        }

        private bool isRefreshed = false;

        private State currentState = State.Unknown;

        public virtual void OnUIReady() {
        }

        public virtual bool Show() {
	        Assert.IsFalse<InvalidOperationException>(isRefreshed, "Cannot Show() popup before Refresh() is called!");

	        //this can happen if the person tries to make the same UI visible twice.
	        //most likely scenario is when the game is ran from a random scene
	        //example -> loading screen being shown twice where it was meant to be hidden in the previous scene
	        if(currentState == State.Showing) {
		        Logging.LogWarning(this, $"Cannot show [{GetType().FullName}], when it's already being shown!");
		        return false;
	        }
	        
	        currentState = State.Showing;
	        
	        Logging.Log(this, $"Showing UI [{GetType().FullName}].");
	        return true;
        }

		public virtual bool Hide() {
			
			//this can happen if the person tries to make the same UI hidden twice.
			//most likely scenario is when the game is ran from a random scene
			//example -> loading screen being hidden twice where it was meant to be shown in the previous scene
			if(currentState == State.Hidden) {
				Logging.LogWarning(this, $"Cannot hide [{GetType().FullName}], when it's already hidden!");
				return false;
			}

			currentState = State.Hidden;
			isRefreshed = false;
			
			Logging.Log(this, $"Hiding UI [{GetType().FullName}].");

			return true;
		}

        public virtual void Refresh() {
			//int selectedMainCanvasSortingOrder = mainCanvas.sortingOrder;
			//string mainCanvasSortingLayer = mainCanvas.sortingLayerName;

			//int mainCanvasSortingOrder = uiManager.CreateSortingOrder(mainCanvas.sortingLayerName);
			//mainCanvas.sortingOrder = mainCanvasSortingOrder;

			//foreach(Canvas subCanvas in subCanvases) {
			//	if(subCanvas.overrideSorting) {
			//		Assert.IsFalse<InvalidOperationException>(mainCanvasSortingLayer.Equals(subCanvas.sortingLayerName), "Canvas cannot have two different sorting layers! Use SortingOrder instead! Main layer: " + mainCanvasSortingLayer + " Second layer: " + subCanvas.sortingLayerName);

			//		int selectedSubCanvasSortingOrder = subCanvas.sortingOrder;
			//		int sortingOrderRelativeDifference = selectedSubCanvasSortingOrder - selectedMainCanvasSortingOrder;

			//		subCanvas.sortingOrder = uiManager.CreateSortingOrder(subCanvas.sortingLayerName, sortingOrderRelativeDifference);
			//	}
			//}

			isRefreshed = true;
        }

	}

}