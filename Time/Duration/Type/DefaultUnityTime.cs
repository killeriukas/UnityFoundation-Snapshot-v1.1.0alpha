using System;
using UnityEngine;

namespace TMI.TimeManagement {

    public class DefaultUnityTime : ITimeDuration {
        public TimeSpan now => TimeSpan.FromSeconds(Time.time);
    }

}