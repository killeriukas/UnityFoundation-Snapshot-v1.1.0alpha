using TMI.Notification;
using System;

namespace TMI.Core {

	public abstract class BaseNotificationManager : BaseManager, INotificationListener {

		private INotificationManager notificationManager;

		public override void Setup(IInitializer initializer, bool isNew) {
			base.Setup(initializer, isNew);
			notificationManager = initializer.GetManager<NotificationManager, INotificationManager>();
		}

		protected void Trigger(INotification notification) {
			notificationManager.Trigger(notification);
		}

		protected void Listen<TNotification>(INotificationListener listener, Action<TNotification> callback) where TNotification : INotification {
			notificationManager.Listen<TNotification>(listener, callback);
		}

        protected void Listen<TNotification>(INotificationListener listener, Action<TNotification> callback, Func<TNotification, bool> predicate) where TNotification : BasePredicateNotification {
            notificationManager.Listen<TNotification>(listener, callback, predicate);
        }

        protected void Listen<TNotification, TPredicate>(INotificationListener listener, Action<TNotification> callback, Func<TPredicate, bool> predicate) where TNotification : BasePredicateNotification where TPredicate : BasePredicateNotification {
            notificationManager.Listen<TNotification, TPredicate>(listener, callback, predicate);
        }

        protected void StopListen<TNotification>(INotificationListener listener) where TNotification : INotification {
			notificationManager.StopListen<TNotification>(listener);
		}

        protected override void OnDestroy() {
            notificationManager.StopListenAll(this);
            base.OnDestroy();
        }

    }

}