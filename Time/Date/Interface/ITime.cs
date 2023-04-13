using System;

namespace TMI.TimeManagement {

    public interface ITime {
        DateTime now { get; }
    }

}