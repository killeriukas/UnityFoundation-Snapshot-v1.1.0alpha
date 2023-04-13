using System;

namespace TMI.Core {

    //maybe later allow the managers to be selected within the config file if there is more than 1 manager available
    
    public interface IInitializer {
        TManagerInterface GetManager<TManager, TManagerInterface>() where TManager : BaseManager, TManagerInterface, new() where TManagerInterface : class, IManager;
        TManagerInterface GetManager<TManagerInterface>(Type managerType) where TManagerInterface : class, IManager;
    }

}