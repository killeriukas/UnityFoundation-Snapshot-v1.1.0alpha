using System;
using UnityEngine;
using System.Collections.Generic;

namespace TMI.UI.Extension {

	public class List : UIComponent {

		[SerializeField]
		private Cell cloneCell;

		private Dictionary<int, Cell> cellsByInstanceId = new Dictionary<int, Cell>();

        protected override void OnInitialize() {
            base.OnInitialize();
            Assert.IsNull(cloneCell, "Clone cell cannot be null! Name: " + name);
            cloneCell.gameObject.SetActive(false);
        }

		public Cell CloneCell() {
			GameObject newCellGo = GameObject.Instantiate(cloneCell.gameObject, base.transform, false);

            //initialize all the TOP components of the cell game object including the cell itself
            IUIComponent[] topUIComponents = newCellGo.GetComponents<IUIComponent>();
            foreach(IUIComponent uiComponent in topUIComponents) {
                uiComponent.OnCloneComplete();
                uiComponent.Setup(initializer);
            }

            Cell newCell = newCellGo.GetComponent<Cell>();
            newCellGo.SetActive(true);

            int instanceId = newCell.GetInstanceID();
			cellsByInstanceId.Add(instanceId, newCell);
			return newCell;
		}

		public void DestroyCell(Cell cell) {
			int instanceId = cell.GetInstanceID();
			bool wasRemoved = cellsByInstanceId.Remove(instanceId);
			if(wasRemoved) {
				GameObject.Destroy(cell.gameObject);
			} else {
				throw new ArgumentException("Cell was not created through this list. InstanceId: " + instanceId + " Name: " + cell.name);
			}
		}

	}

}