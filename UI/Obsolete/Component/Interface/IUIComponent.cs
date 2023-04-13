using TMI.Core;

namespace TMI.UI {

    public interface IUIComponent {
        void Setup(IInitializer initializer);
        void OnCloneComplete();
    }

}
