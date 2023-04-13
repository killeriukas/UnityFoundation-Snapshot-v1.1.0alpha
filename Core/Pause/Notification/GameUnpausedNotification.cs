using TMI.Notification;
using TMI.TimeManagement;

namespace TMI.Core {

    public class GameUnpausedNotification : BaseValueNotification<MonotonicTime> {

        public GameUnpausedNotification(MonotonicTime unpauseTime) : base(unpauseTime) {

        }

    }

}