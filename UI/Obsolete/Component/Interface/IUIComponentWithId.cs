
namespace TMI.UI {

	public interface IUIComponentWithId : IUIComponent {
        string id { get; }
		string name { get; }
	}

}