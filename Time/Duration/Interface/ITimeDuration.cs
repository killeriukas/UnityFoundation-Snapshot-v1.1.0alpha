using System;

namespace TMI.TimeManagement {

    public interface ITimeDuration {
        TimeSpan now { get; }
    }

}