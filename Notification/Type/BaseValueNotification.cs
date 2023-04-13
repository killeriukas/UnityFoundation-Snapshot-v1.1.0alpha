
namespace TMI.Notification {

	public abstract class BaseValueNotification<TValueType> : INotification {

		public readonly TValueType value;

		protected BaseValueNotification(TValueType value) {
			this.value = value;
		}

	}

}