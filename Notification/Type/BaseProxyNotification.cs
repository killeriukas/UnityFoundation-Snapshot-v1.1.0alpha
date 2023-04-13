using TMI.Pattern;

namespace TMI.Notification {

	public abstract class BaseProxyNotification<TProxy> : INotification where TProxy : IProxy {

		public readonly TProxy proxy;

		protected BaseProxyNotification(TProxy proxy) {
			this.proxy = proxy;
		}


	}


}