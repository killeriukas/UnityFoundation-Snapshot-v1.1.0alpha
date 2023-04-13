namespace TMI.LogManagement {

    public interface ILogOutput {
        void Log(string categoryType, string message);
        void Log(object loggable, string message);
        void LogWarning(string categoryType, string message);
        void LogWarning(object loggable, string message);
        void LogError(string categoryType, string message);
        void LogError(object loggable, string message);
    }

}
