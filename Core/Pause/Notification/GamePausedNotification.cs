using TMI.Notification;
using TMI.TimeManagement;

namespace TMI.Core {

    public class GamePausedNotification : BaseValueNotification<MonotonicTime> {

        public readonly bool hasQuitApplication;

        public GamePausedNotification(MonotonicTime pauseTime, bool hasQuitApplication) : base(pauseTime) {
            this.hasQuitApplication = hasQuitApplication;
        }

    }

}