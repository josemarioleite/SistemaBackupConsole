using System.Collections.Generic;
using System.IO;

namespace SistemaBackupConsole.Models
{
    public class FileConfiguration
    {
        private static string nameFile = "fileConfiguration.txt";
        private static string currentPath = Directory.GetCurrentDirectory();
        private static string file = Path.Combine(currentPath, nameFile);
        public bool FileExists { get; set; }
        public string FileConfig
        {
            get
            {
                return file;
            }
        }
        public List<string> DataFile { get; set; }
    }
}
