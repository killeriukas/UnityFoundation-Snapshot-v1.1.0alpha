using System;

namespace TMI.Core {

	public abstract class BaseObject : IDisposable {

		protected IInitializer initializer { get; private set; }

		protected BaseObject(IInitializer initializer) {
			this.initializer = initializer;
		}

        public virtual void Dispose() {
        }

    }

}