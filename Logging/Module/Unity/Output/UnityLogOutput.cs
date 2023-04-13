using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TMI.LogManagement.Unity {
    
    [CreateAssetMenu(fileName = "UnityLogOutput", menuName = "TMI/Config/Module/Log/Output/UnityDefault", order = 0)]
    public class UnityLogOutput : BaseLogOutput
    {

        public override void Log(string categoryType, string message) {
            Debug.Log(message);
        }

        public override void Log(object loggable, string message) {
            Debug.Log(message);
        }

        public override void LogWarning(string categoryType, string message) {
            Debug.LogWarning(message);
        }

        public override void LogWarning(object loggable, string message) {
            Debug.LogWarning(message);
        }

        public override void LogError(object loggable, string message) {
            Debug.LogError(message);
        }
        
        public override void LogError(string categoryType,  string message) {
            Debug.LogError(message);
        }
        
    }

    
}

