using TMI.Core;
using System;

namespace TMI.Notification {

	public interface INotificationManager : IManager {
		void Trigger(INotification notification);
		void Listen<TNotification>(INotificationListener listener, Action<TNotification> callback) where TNotification : INotification;
        void Listen<TNotification>(INotificationListener listener, Action<TNotification> callback, Func<TNotification, bool> predicate) where TNotification : BasePredicateNotification;
        void Listen<TNotification, TPredicate>(INotificationListener listener, Action<TNotification> callback, Func<TPredicate, bool> predicate) where TNotification : BasePredicateNotification where TPredicate : BasePredicateNotification;
        void StopListen<TNotification>(INotificationListener listener) where TNotification : INotification;
		void StopListenAll(INotificationListener listener);
	}


}