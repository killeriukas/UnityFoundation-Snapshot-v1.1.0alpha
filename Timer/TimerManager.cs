using System;
using System.Collections.Generic;
using System.Linq;
using TMI.AssetManagement;
using TMI.Core;
using TMI.TimeManagement;

namespace TMI.TimerManagement {

    public class TimerManager : BaseManager, ITimerManager {

        private class Timer : IHandle {

            private bool isDestroyed = false;

            private Action onComplete;

            public readonly MonotonicTime expirationTime;
            private readonly TimeSpan duration;

            private TimerManager timerManager;

            public Timer(TimerManager timerManager, TimeSpan duration, Action onComplete) {
                Assert.IsNull(onComplete, "Callback onComplete cannot be null!");

                this.timerManager = timerManager;
                this.onComplete = onComplete;

                this.duration = duration;
                this.expirationTime = MonotonicTime.now + duration;
            }

            public Timer(TimerManager timerManager, MonotonicTime expirationTime, Action onComplete) {
                MonotonicTime now = MonotonicTime.now;
                Assert.IsFalse<InvalidOperationException>(expirationTime > now, "Expiration time cannot be in the past!");

                this.timerManager = timerManager;
                this.onComplete = onComplete;

                this.expirationTime = expirationTime;
                this.duration = expirationTime - now;
            }

            public void MarkExpired() {
                if(!isDestroyed) {
                    onComplete();
                }
            }

            public float progress {
                get {
                    TimeSpan diff = expirationTime - MonotonicTime.now;
                    float param = (float)(1 - diff.TotalSeconds / duration.TotalSeconds);
                    return param;
                }
            }

            public bool Cancel() {
                if(!isDestroyed) {
                    isDestroyed = true;
                    timerManager.Unregister(this);
                    timerManager = null;
                    onComplete = null;
                    return true;
                } else {
                    return false;
                }
            }
        }

        private List<Timer> sortedTimers = new List<Timer>();

        public override void Setup(IInitializer initializer, bool isNew) {
            base.Setup(initializer, isNew);

            if(isNew) {
                RegisterUpdate();
            }
        }

        public IHandle Register(TimeSpan duration, Action onComplete) {
            Timer timer = new Timer(this, duration, onComplete);

            sortedTimers.Add(timer);
            sortedTimers = sortedTimers.OrderByDescending(x => x.expirationTime).ToList();

            return timer;
        }

        public IHandle Register(MonotonicTime expirationTime, Action onComplete) {
            Timer timer = new Timer(this, expirationTime, onComplete);

            sortedTimers.Add(timer);
            sortedTimers = sortedTimers.OrderByDescending(x => x.expirationTime).ToList();

            return timer;
        }

        protected override ExecutionManager.Result OnUpdate() {

            TryRemoveTimer();

            return ExecutionManager.Result.Continue;
        }

        //TODO: make this one better. maybe use DeferredList<> or smth
        private void TryRemoveTimer() {
            if(sortedTimers.Count > 0) {
                int lastIndex = sortedTimers.Count - 1;
                Timer timer = sortedTimers[lastIndex];

                if(timer.expirationTime < MonotonicTime.now) {
                    timer.MarkExpired();
                    timer.Cancel();
                    TryRemoveTimer();
                }

                //TODO: try to stop Update loop here, if sortedTimers list is empty
                //if() {

                //}
            }
        }

        private void Unregister(Timer timer) {
            sortedTimers.Remove(timer);
        }

        protected override void OnDestroy() {
            UnregisterUpdate();
            base.OnDestroy();
        }

    }

}