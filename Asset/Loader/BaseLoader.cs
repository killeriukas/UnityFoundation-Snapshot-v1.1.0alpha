using System.Collections.Generic;

namespace TMI.AssetManagement {

	public abstract class BaseLoader : ILoader, ILoaderComplete {
		public abstract float progress { get; }
		public abstract bool isComplete { get; }
		public abstract bool isEmpty { get; }
		
		public abstract void UnloadAll();
		public abstract LoadHandle LoadAsync(IRequestExecutor executor);
		public abstract bool Unload<TObject>(TObject unloadObject);
		public abstract TObject Get<TObject>(string id);
		public abstract IEnumerable<string> GetAll();
	}

}