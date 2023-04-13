namespace TMI.AssetManagement {

	public interface ILoader {
		float progress { get; }
		
		bool isComplete { get; }

		bool isEmpty { get; }
		
		void UnloadAll();

		LoadHandle LoadAsync(IRequestExecutor executor);

		bool Unload<TObject>(TObject unloadObject);
	}

}