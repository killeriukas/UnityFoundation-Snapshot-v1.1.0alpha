using System;
using System.Collections.Generic;
using TMI.TimeManagement;
using UnityEngine;

namespace TMI.AssetManagement {

    public class FakeLoader : BaseLoader {

        private readonly MonotonicTime endTime;
        private readonly TimeSpan duration;

        public override float progress {
            get {
                MonotonicTime now = MonotonicTime.now;
                TimeSpan timeLeft = endTime - now;
                float durationLeft = (float)timeLeft.TotalSeconds;

                float durationSec = (float)duration.TotalSeconds;

                float p = 1f - Mathf.Clamp01(durationLeft / durationSec);
                return p;
            }
        }

        public override bool isComplete => MonotonicTime.now > endTime;

        public FakeLoader(TimeSpan duration) {
            this.duration = duration;
            this.endTime = MonotonicTime.now + duration;
        }

        public override LoadHandle LoadAsync(IRequestExecutor executor) {
            LoadHandle loadHandle = new LoadHandle(this, executor);
            return loadHandle;
        }

        public override bool isEmpty => true;
        
        public override void UnloadAll() => throw new InvalidOperationException("This is a FakeLoader!");
        public override bool Unload<TObject>(TObject unloadObject) => throw new InvalidOperationException("This is a FakeLoader!");
        public override TObject Get<TObject>(string id) => throw new InvalidOperationException("This is a FakeLoader!");
        public override IEnumerable<string> GetAll() => throw new InvalidOperationException("This is a FakeLoader!");
    }

}