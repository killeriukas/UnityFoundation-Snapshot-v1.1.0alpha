using Newtonsoft.Json;
using TMI.LogManagement;

namespace TMI.Helper {

    [Loggable]
    public static class SerializationHelper {

        private static readonly string fullTypeName = typeof(SerializationHelper).FullName;

        public static Formatting currentFormatting = Formatting.None;

        private static readonly JsonSerializerSettings defaultSettings = new JsonSerializerSettings() {
            TypeNameHandling = TypeNameHandling.Auto
        };

        public static string Serialize(object item) {
            Logging.Log(fullTypeName, "Started file [{0}] serialization.", item.GetType().Name);
            string content = JsonConvert.SerializeObject(item, currentFormatting, defaultSettings);
            Logging.Log(fullTypeName, "Serialized file with content [{0}].", content);
            return content;
        }

        public static TObject Deserialize<TObject>(string content) {
            Logging.Log(fullTypeName, "Deserializing file from content [{0}].", content);
            TObject item = JsonConvert.DeserializeObject<TObject>(content, defaultSettings);
            Logging.Log(fullTypeName, "Deserialization finished.");
            return item;
        }

    }

}

