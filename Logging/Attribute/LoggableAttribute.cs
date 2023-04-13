using System;

namespace TMI.LogManagement {

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, AllowMultiple = false, Inherited = false)]
    public class LoggableAttribute : Attribute {
        public bool isMandatory { get; set; } = false;
        public string groupName { get; set; } = string.Empty;
    }

}
