using UnityEngine;

namespace TMI.LogManagement.Unity {
    
    [CreateAssetMenu(fileName = "UnityConditionalLogOutput", menuName = "TMI/Config/Module/Log/Output/UnityConditional", order = 0)]
    public class UnityConditionalLogOutput : BaseConditionalLogOutput
    {
        
        protected override void InternalLog(string categoryType, string message) {
            Debug.Log(message);
        }
        
        protected override void InternalLog(object loggable, string message) {
            Debug.Log(message);
        }
        
        protected override void InternalLogWarning(string categoryType, string message) {
            Debug.LogWarning(message);
        }

        protected override void InternalLogWarning(object loggable, string message) {
            Debug.LogWarning(message);
        }

        protected override void InternalLogError(string categoryType, string message) {
            Debug.LogError(message);
        }
        
        protected override void InternalLogError(object loggable, string message) {
            Debug.LogError(message);
        }
        
    }

    
}

