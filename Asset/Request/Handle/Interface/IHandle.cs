
namespace TMI.AssetManagement {
	
	public interface IHandle {
		float progress { get; }
		bool Cancel();
	}

}