using System;

//re-think this later on
#if USE_NEWTONSOFT_JSON
using Newtonsoft.Json;
#endif

namespace TMI.TimeManagement {

    //re-think this later on
    #if USE_NEWTONSOFT_JSON
    [JsonObject(MemberSerialization.OptIn)]
    #endif
    public struct MonotonicTime : IComparable<MonotonicTime> {

        //re-think this later on
        #if USE_NEWTONSOFT_JSON
        [JsonProperty]
        #endif
        private DateTime dateTime;

        private static DateTime epochStartTime = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        public MonotonicTime(DateTime dateTime) {
            this.dateTime = dateTime;
        }

        public static ITime timeReceiver = new DefaultSystemTime();

        public static MonotonicTime now {
            get {
                DateTime utcNow = timeReceiver.now;
                MonotonicTime monotonicTime = new MonotonicTime(utcNow);
                return monotonicTime;
            }
        }

        public static TimeSpan operator-(MonotonicTime mt1, MonotonicTime mt2) {
            TimeSpan difference = mt1.dateTime.Subtract(mt2.dateTime);
            return difference;
        }

        public static MonotonicTime operator -(MonotonicTime mt, TimeSpan duration) {
            DateTime dateTimeSub = mt.dateTime.Subtract(duration);
            MonotonicTime monotonicSub = new MonotonicTime(dateTimeSub);
            return monotonicSub;
        }

        public static MonotonicTime operator +(MonotonicTime mt, TimeSpan duration) {
            DateTime dateTimeSum = mt.dateTime.Add(duration);
            MonotonicTime monotonicSum = new MonotonicTime(dateTimeSum);
            return monotonicSum;
        }

        public static bool operator >=(MonotonicTime mt1, MonotonicTime mt2) {
            bool isMore = mt1.dateTime >= mt2.dateTime;
            return isMore;
        }

        public static bool operator <=(MonotonicTime mt1, MonotonicTime mt2) {
            bool isLess = mt1.dateTime <= mt2.dateTime;
            return isLess;
        }

        public static bool operator>(MonotonicTime mt1, MonotonicTime mt2) {
            bool isMore = mt1.dateTime > mt2.dateTime;
            return isMore;
        }

        public static bool operator<(MonotonicTime mt1, MonotonicTime mt2) {
            bool isLess = mt1.dateTime < mt2.dateTime;
            return isLess;
        }

        public double epochTime {
            get {
                TimeSpan diff = dateTime - epochStartTime;
                return diff.TotalSeconds;
            }
        }

        public override string ToString() {
            return dateTime.ToString();
        }

        public int CompareTo(MonotonicTime other) {
            return dateTime.CompareTo(other.dateTime);
        }
    }


}