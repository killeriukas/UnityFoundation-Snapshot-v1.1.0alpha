using UnityEngine.UI;

namespace TMI.UI.Extension {

    public abstract class BaseNoGraphic : Graphic {

        sealed protected override void OnPopulateMesh(VertexHelper vh) {
            vh.Clear();
        }

    }
        
}