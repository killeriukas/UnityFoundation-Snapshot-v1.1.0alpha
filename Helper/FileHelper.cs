using System.IO;

namespace TMI.Helper {

    public static class FileHelper {

        private const string tempFileSuffix = "-temp";
        private const string backupFileSuffix = "-bac";

        public static void WriteWithBackup(string filePath, string content) {

            bool fileExists = File.Exists(filePath);

            //the operations are atomic - if computer crashed whilst writing to a file, it would destroy backup only, but not the main save file
            //if computer crashed whilst replacing the file - replacing will not happen and the main save file + backup will stay safe!
            if(fileExists) {
                //if file already exists, write new data into the temp file first
                string tempFileName = filePath + tempFileSuffix;
                File.WriteAllText(tempFileName, content);

                //replace the temporary file with original file and create a backup of old file
                string backupFileName = filePath + backupFileSuffix;
                File.Replace(tempFileName, filePath, backupFileName);

                //delete the backup of old file
                File.Delete(backupFileName);
            } else {
                //if no files are available yet, there is nothing else to do, but to write directly to the file. There is nothing to look after yet
                File.WriteAllText(filePath, content);
            }
        }

        public static string ReadWithBackup(string filePath) {

            string tempFileName = filePath + tempFileSuffix;

            if(File.Exists(tempFileName)) {

                //replace the backup file with original file
                string backupFileName = filePath + backupFileSuffix;
                File.Replace(tempFileName, filePath, backupFileName);

                //delete the backup
                File.Delete(backupFileName);
            }

            string content = File.ReadAllText(filePath);
            return content;
        }

        public static bool ExistsBasedOnPlatform(string filePath) {
#if UNITY_EDITOR
            return File.Exists(filePath);
#else
            string fileName = Path.GetFileNameWithoutExtension(filePath);
            object loadedFile = UnityEngine.Resources.Load(fileName);
            return loadedFile != null;
#endif
        }


    }


}