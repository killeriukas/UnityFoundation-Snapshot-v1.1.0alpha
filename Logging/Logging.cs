using System.Collections.Generic;
using UnityEngine;

namespace TMI.LogManagement {

    [Loggable(isMandatory = true)]
    public static class Logging {

        private static bool isInitialized = false;
        
        //default print to unity log
        private static List<ILogOutput> output = new List<ILogOutput>() {
            new UnityLogOutput()
        };

        public static void AddLogOutput(ILogOutput logOutput) {
            if(!isInitialized) {
                Log(typeof(Logging).FullName, "Clearing the default logging output! No panic, if overriden by yourself.");
                isInitialized = true;
                output.Clear();
            }
            output.Add(logOutput);
        }
        
        public static void AddLogOutput(IEnumerable<ILogOutput> logOutput) {
            if(!isInitialized) {
                Log(typeof(Logging).FullName, "Clearing the default logging output! No panic, if overriden by yourself.");
                isInitialized = true;
                output.Clear();
            }
            output.AddRange(logOutput);
        }
        
        [System.Diagnostics.Conditional("ENABLE_LOGGING")]
        public static void Log(string typeFullName, string unformattedString, params object[] moreParameters) {
            string formattedString = string.Format(unformattedString, moreParameters);

            foreach(ILogOutput logOutput in output) {
                logOutput.Log(typeFullName, formattedString);    
            }
        }

        [System.Diagnostics.Conditional("ENABLE_LOGGING")]
        public static void Log(object loggableObject, string unformattedString, params object[] moreParameters) {
            string formattedString = string.Format(unformattedString, moreParameters);
            
            foreach(ILogOutput logOutput in output) {
                logOutput.Log(loggableObject, formattedString);    
            }
        }

        [System.Diagnostics.Conditional("ENABLE_LOGGING")]
        public static void LogWarning(object loggableObject, string unformattedString, params object[] moreParameters) {
            string formattedString = string.Format(unformattedString, moreParameters);
            
            foreach(ILogOutput logOutput in output) {
                logOutput.LogWarning(loggableObject, formattedString);    
            }
        }

        [System.Diagnostics.Conditional("ENABLE_LOGGING")]
        public static void LogWarning(string typeFullName, string unformattedString, params object[] moreParameters) {
            string formattedString = string.Format(unformattedString, moreParameters);

            foreach(ILogOutput logOutput in output) {
                logOutput.LogWarning(typeFullName, formattedString);    
            }
        }

        [System.Diagnostics.Conditional("ENABLE_LOGGING")]
        public static void LogError(object loggableObject, string unformattedString, params object[] moreParameters) {
            string formattedString = string.Format(unformattedString, moreParameters);

            foreach(ILogOutput logOutput in output) {
                logOutput.LogError(loggableObject, formattedString);
            }
        }

        [System.Diagnostics.Conditional("ENABLE_LOGGING")]
        public static void LogError(string typeFullName, string unformattedString, params object[] moreParameters) {
            string formattedString = string.Format(unformattedString, moreParameters);

            foreach(ILogOutput logOutput in output) {
                logOutput.LogError(typeFullName, formattedString);
            }
        }
        
    }

}