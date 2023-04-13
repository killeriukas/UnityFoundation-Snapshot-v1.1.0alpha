using System;

namespace TMI.AssetManagement {

    public class RequestHandler : IRequestHandler, IRequestExecutor {

		#region IRequestHandler implementation
		public event Action<ILoaderComplete> onCompleted;
		public event Action onCanceled;
		public event Action onFailed;
		#endregion

		private RequestHandler() {

		}

		private RequestHandler(Action<ILoaderComplete> completedCallback) {
			onCompleted += completedCallback;
		}

		public static IRequestHandler Create(Action<ILoaderComplete> completedCallback) {
			return new RequestHandler(completedCallback);
		}

		public static IRequestHandler Create() {
			return new RequestHandler();
		}

		public IRequestExecutor GetExecutor() {
			return this;
		}
		
		#region IRequestExecutor implementation
		public void Complete(ILoaderComplete loadedAsset) {
            onCompleted?.Invoke(loadedAsset);
		}
		public void Cancel() {
            onCanceled?.Invoke();
		}
		public void Fail() {
            onFailed?.Invoke();
		}
		#endregion
	}

}