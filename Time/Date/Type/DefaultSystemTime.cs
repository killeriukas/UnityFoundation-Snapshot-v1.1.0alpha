using System;

namespace TMI.TimeManagement {

    public class DefaultSystemTime : ITime {

        public DateTime now {
            get {
                return DateTime.UtcNow;
            }
        }

    }

}