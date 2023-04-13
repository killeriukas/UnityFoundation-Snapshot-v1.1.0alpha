using System;
using UnityEngine;
using System.Collections.Generic;
using TMI.Core;
using TMI.Helper;

namespace TMI.UI.Extension {

	public class Cell : UIComponent {

		private Dictionary<string, IUIComponentWithId> uiComponentById = new Dictionary<string, IUIComponentWithId>();

        protected override void OnInitialize() {
            base.OnInitialize();

            //on clone complete
            IEnumerable<IUIComponent> uiComponentsWithoutCells = HierarchyHelper.GetComponentsInChildrenOnlyExcludingOne<IUIComponent, Cell>(this);

            foreach(IUIComponent uiComponent in uiComponentsWithoutCells) {
                uiComponent.OnCloneComplete();
            }

            //get all TMI.UI components with IDS only, check if all of them have IDS, and store them for easier search later on
            //take Cell gameobject components with ids too and put them inside the list, cuz cell is resposible for them too
            IEnumerable<IUIComponentWithId> uiComponentsWithIdsWithoutCells = HierarchyHelper.GetComponentsInChildrenExcludingOne<IUIComponentWithId, Cell>(this);
            foreach(IUIComponentWithId uiComponent in uiComponentsWithIdsWithoutCells) {
                Assert.IsTrue<ArgumentException>(string.IsNullOrEmpty(uiComponent.id), "Component id is null! Name: " + uiComponent.name);
                Assert.IsTrue<ArgumentException>(uiComponentById.ContainsKey(uiComponent.id), "The same key element has been found. Id: " + uiComponent.id  + " Name: " + uiComponent.name);
                uiComponentById.Add(uiComponent.id, uiComponent);
            }
        }

        public override void Setup(IInitializer initializer) {
            base.Setup(initializer);

            //get all TMI.UI components and their extensions and set them up without including other cells and their children
            IEnumerable<IUIComponent> uiComponentsWithoutCells = HierarchyHelper.GetComponentsInChildrenOnlyExcludingOne<IUIComponent, Cell>(this);

            foreach(IUIComponent uiComponent in uiComponentsWithoutCells) {
                uiComponent.Setup(initializer);
            }
        }

        public TComponent FindComponent<TComponent>(string id) where TComponent : IUIComponentWithId {
            return (TComponent)uiComponentById[id];
        }


        //RETHINK THE STRATEGY BELOW!!!!
        private TimeSpan destructionTime;
		private TimeSpan totalDeltaTime;
		private UIList list;

        //TODO: change this into the update executioner
        //TODO: cell cannot update itself. Think of any other way of doing this!!!
        private void Update() {
			if(list != null) {
				TimeSpan deltaTime = TimeSpan.FromSeconds(Time.deltaTime);

				totalDeltaTime = totalDeltaTime.Add(deltaTime);

				if(totalDeltaTime > destructionTime) {
					list.DestroyCell(this);
					list = null;
				}
			}	
		}

        //TODO: cell cannot destroy itself. Think of any other way of doing this!!!
		public void MarkAsSelfDestroy(UIList cellList, TimeSpan time) {
			this.list = cellList;
			this.destructionTime = time;
		}


	}

}