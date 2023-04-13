using System;

namespace TMI.AssetManagement {

    public class FakeGroup : IGroup {

        private TimeSpan duration;

        public FakeGroup(TimeSpan duration) {
            this.duration = duration;
        }
        
        public ILoader CreateLoader() {
            FakeLoader fakeLoader = new FakeLoader(duration);
            return fakeLoader;
        }
        
    }

}