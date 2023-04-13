using System;

namespace TMI.Core {

	public interface IManager {
		void Setup(IInitializer initializer, bool isNew);
	}

}


