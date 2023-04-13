using UnityEngine;

namespace TMI.LogManagement {

    public class UnityLogOutput : ILogOutput {

        public void Log(string categoryType, string message) {
            Debug.Log(message);
        }

        public void Log(object loggable, string message) {
            Debug.Log(message);
        }

        public void LogWarning(string categoryType, string message) {
            Debug.LogWarning(message);
        }

        public void LogWarning(object loggable, string message) {
            Debug.LogWarning(message);
        }
        public void LogError(string categoryType, string message) {
            Debug.LogError(message);
        }

        public void LogError(object loggable, string message) {
            Debug.LogError(message);
        }

    }

}