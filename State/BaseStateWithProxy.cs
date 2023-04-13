using TMI.Pattern;
using TMI.Core;

namespace TMI.State {
	
	public abstract class BaseStateWithProxy<TProxy> : BaseState where TProxy : IProxy {

		protected readonly TProxy proxy;

		protected BaseStateWithProxy(IInitializer initializer, TProxy proxy) : base(initializer) {
			this.proxy = proxy;
		}

	}

}

