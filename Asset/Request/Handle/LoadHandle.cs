namespace TMI.AssetManagement {

	public class LoadHandle : IHandle {

		private IRequestExecutor requestExecutor;
		private BaseLoader loader;
		
		public bool isValid => loader.isComplete && !isCanceled;
		public float progress => loader.progress;
		public bool isCanceled { get; private set; }
		
		public LoadHandle(BaseLoader loader, IRequestExecutor requestExecutor) {
			this.loader = loader;
			this.requestExecutor = requestExecutor;
			this.isCanceled = false;
		}

		public void Complete() {
			requestExecutor.Complete(loader);
		}

		private void OnCancel() {
			requestExecutor.Cancel();
		}

		public bool Cancel() {
			
			//can't cancel. it's too late
			if(loader.isComplete) {
				return false;
			}

			//can't double cancel
			if(isCanceled) {
				return false;
			}
			
			isCanceled = true;
			OnCancel();
			return true;
		}

	}
}