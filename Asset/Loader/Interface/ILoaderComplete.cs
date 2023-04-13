using System.Collections.Generic;

namespace TMI.AssetManagement {

	public interface ILoaderComplete {
		TObject Get<TObject>(string id);
		
		IEnumerable<string> GetAll();
	}

}