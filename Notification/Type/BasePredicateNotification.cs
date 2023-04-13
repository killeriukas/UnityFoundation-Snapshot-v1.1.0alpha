namespace TMI.Notification {

    public abstract class BasePredicateNotification : INotification {

        private readonly string _notificationId;

        protected BasePredicateNotification(string notificationId) {
            this._notificationId = notificationId;
        }

        public bool IsMyNotification(string notificationId) {
            return _notificationId.Equals(notificationId);
        }
    }

}