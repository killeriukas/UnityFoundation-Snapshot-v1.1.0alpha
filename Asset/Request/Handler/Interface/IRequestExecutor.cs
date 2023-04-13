namespace TMI.AssetManagement {

    public interface IRequestExecutor {
		void Complete(ILoaderComplete loadedAsset);
		void Cancel();
		void Fail();
	}

}