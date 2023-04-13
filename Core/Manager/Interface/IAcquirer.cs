using System;

namespace TMI.Core {

    public interface IAcquirer {
        TManagerInterface AcquireManager<TManagerInterface>(Type managerType) where TManagerInterface : IManager;
        TManagerInterface AcquireManager<TManager, TManagerInterface>() where TManager : BaseManager, TManagerInterface where TManagerInterface : IManager;
    }

}