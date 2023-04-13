
namespace TMI.Notification {

	public abstract class BaseValueChangeNotification<TValueType> : INotification {
		
		public readonly TValueType oldValue;
		public readonly TValueType newValue;

		protected BaseValueChangeNotification(TValueType oldValue, TValueType newValue) {
			this.oldValue = oldValue;
			this.newValue = newValue;
		}

	}

}