using UnityEngine;

namespace TMI.LogManagement.Unity {
 
    public abstract class BaseLogOutput : ScriptableObject, ILogOutput
    {
        public abstract void Log(string categoryType, string message);
        public abstract void Log(object loggable, string message);
        public abstract void LogWarning(string categoryType, string message);
        public abstract void LogWarning(object loggable, string message);
        public abstract void LogError(object loggable, string message);
        
        public abstract void LogError(string categoryType, string message);
    }

}

