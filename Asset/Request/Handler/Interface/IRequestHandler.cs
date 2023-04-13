using System;

namespace TMI.AssetManagement {

	public interface IRequestHandler : IRequestJoiner {
		event Action<ILoaderComplete> onCompleted;
		event Action onCanceled;
		event Action onFailed;
	}

}

