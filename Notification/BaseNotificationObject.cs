using TMI.Core;
using System;

namespace TMI.Notification {

	public abstract class BaseNotificationObject : BaseObject, INotificationListener {

		private INotificationManager notificationManager;

		protected BaseNotificationObject(IInitializer initializer) : base(initializer) {
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

		protected void StopListenAll(INotificationListener listener) {
			notificationManager.StopListenAll(listener);
		}

        public override void Dispose() {
            StopListenAll(this);
            base.Dispose();
        }
    }


}