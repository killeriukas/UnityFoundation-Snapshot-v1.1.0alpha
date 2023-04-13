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
    public struct GameTime : IComparable<GameTime> {

        //re-think this later on
        #if USE_NEWTONSOFT_JSON
        [JsonProperty]
        #endif
        private TimeSpan duration;

        public GameTime(TimeSpan duration) {
            this.duration = duration;
        }

        public static ITimeDuration timeReceiver = new DefaultUnityTime();

        public static GameTime now {
            get {
                TimeSpan now = timeReceiver.now;
                GameTime gameTimeNow = new GameTime(now);
                return gameTimeNow;
            }
        }

        public static TimeSpan operator -(GameTime gt1, GameTime gt2) {
            TimeSpan difference = gt1.duration.Subtract(gt2.duration);
            return difference;
        }

        //public static MonotonicTime operator -(MonotonicTime mt, TimeSpan duration) {
        //    DateTime dateTimeSub = mt.dateTime.Subtract(duration);
        //    MonotonicTime monotonicSub = new MonotonicTime(dateTimeSub);
        //    return monotonicSub;
        //}

        //public static MonotonicTime operator +(MonotonicTime mt, TimeSpan duration) {
        //    DateTime dateTimeSum = mt.dateTime.Add(duration);
        //    MonotonicTime monotonicSum = new MonotonicTime(dateTimeSum);
        //    return monotonicSum;
        //}

        //public static bool operator >=(MonotonicTime mt1, MonotonicTime mt2) {
        //    bool isMore = mt1.dateTime >= mt2.dateTime;
        //    return isMore;
        //}

        //public static bool operator <=(MonotonicTime mt1, MonotonicTime mt2) {
        //    bool isLess = mt1.dateTime <= mt2.dateTime;
        //    return isLess;
        //}

        //public static bool operator>(MonotonicTime mt1, MonotonicTime mt2) {
        //    bool isMore = mt1.dateTime > mt2.dateTime;
        //    return isMore;
        //}

        //public static bool operator<(MonotonicTime mt1, MonotonicTime mt2) {
        //    bool isLess = mt1.dateTime < mt2.dateTime;
        //    return isLess;
        //}

        public override string ToString() {
            return duration.ToString();
        }

        public int CompareTo(GameTime other) {
            return duration.CompareTo(other.duration);
        }
    }


}