using UnityEngine;
using TMI.UI.Extension;

namespace TMI.UI {

    [RequireComponent(typeof(List))]
    public class UIList : UIComponentWithId<List> {

        public Cell CloneCell() {
            return component.CloneCell();
        }

        public void DestroyCell(Cell cell) {
            component.DestroyCell(cell);
        }

    }
}