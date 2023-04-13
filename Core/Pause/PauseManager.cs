using System;
using TMI.LogManagement;
using TMI.TimeManagement;

namespace TMI.Core {

    [Loggable]
    public class PauseManager : BaseNotificationManager, IPauseManager {

        private enum State {
            Starting,
            Running
        }

        private State currentState = State.Starting;

        private void OnApplicationPause(bool pause) {
            switch(currentState) {
                case State.Starting:
                    currentState = State.Running;
                    Logging.Log(this, "The game has just launched. PauseManager() was moved to the state [{0}].", currentState);
                    break;
                case State.Running:
                    MonotonicTime timeNow = MonotonicTime.now;
                    if(pause) {
                        Logging.Log(this, "The game was paused at [{0}].", timeNow);
                        Trigger(new GamePausedNotification(timeNow, false));
                    } else {
                        Logging.Log(this, "The game was unpaused at [{0}].", timeNow);
                        Trigger(new GameUnpausedNotification(timeNow));
                    }
                    break;
                default:
                    throw new ArgumentException("There is no argument you are looking for: " + currentState);
            }
        }

        private void OnApplicationQuit() {
            MonotonicTime timeNow = MonotonicTime.now;
            Logging.Log(this, "The game was paused by quitting at [{0}].", timeNow);
            Trigger(new GamePausedNotification(timeNow, true));
        }

    }

}