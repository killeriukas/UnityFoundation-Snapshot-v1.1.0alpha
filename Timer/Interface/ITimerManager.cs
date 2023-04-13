using System;
using TMI.AssetManagement;
using TMI.Core;
using TMI.TimeManagement;

namespace TMI.TimerManagement {

    public interface ITimerManager : IManager {
        IHandle Register(TimeSpan duration, Action onComplete);
        IHandle Register(MonotonicTime expirationTime, Action onComplete);

    }

}