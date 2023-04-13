using System;
using TMI.ConfigManagement.Unity.Log;
using UnityEngine;

namespace TMI.LogManagement.Unity {
    
    public abstract class BaseConditionalLogOutput : BaseLogOutput
    {

        [SerializeField]
        private LoggingConfig loggingConfig;

        private bool ShouldLogMessage(string typeFullName) {
            bool shouldPrint;
            bool hasFoundValue = loggingConfig.TryGetValue(typeFullName, out shouldPrint);

            //TODO: fix this to Logging logging. Make sure it prints out a warning as expected
            if(!hasFoundValue) {
                Debug.LogWarning("It's time to rebuild LoggingConfig(). Missing file: " + typeFullName);
            }

            return shouldPrint;
        }

        private bool ShouldLogMessage(object loggable) {
            Type type = loggable.GetType();
            string typeFullName = type.FullName;
            return ShouldLogMessage(typeFullName);
        }

        public override void Log(string categoryType, string message) {
            if(ShouldLogMessage(categoryType)) {
                InternalLog(categoryType, message);
            }
        }

        public override void Log(object loggable, string message) {
            if(ShouldLogMessage(loggable)) {
                InternalLog(loggable, message);
            }
        }

        public override void LogWarning(string categoryType, string message) {
            if(ShouldLogMessage(categoryType)) {
                InternalLogWarning(categoryType, message);
            }
        }

        public override void LogWarning(object loggable, string message) {
            if(ShouldLogMessage(loggable)) {
                InternalLogWarning(loggable, message);
            }
        }

        public override void LogError(string categoryType, string message) {
            if(ShouldLogMessage(categoryType)) {
                InternalLogError(categoryType, message);
            }
        }
        
        public override void LogError(object loggable, string message) {
            if(ShouldLogMessage(loggable)) {
                InternalLogError(loggable, message);
            }
        }

        protected abstract void InternalLog(string categoryType, string message);
        protected abstract void InternalLog(object loggable, string message);
        protected abstract void InternalLogWarning(object loggable, string message);
        protected abstract void InternalLogWarning(string categoryType, string message);
        protected abstract void InternalLogError(string categoryType, string message);
        protected abstract void InternalLogError(object loggable, string message);
        
    }

    
}

