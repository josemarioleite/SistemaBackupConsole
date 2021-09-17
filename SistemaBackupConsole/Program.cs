using SistemaBackupConsole.Services;

namespace SistemaBackupConsole
{
    public class Program
    {
        public static void Main(string[] args)
        {
            BackupService backupDatabase = new();
            backupDatabase.ExecuteBackupDatabase();
            backupDatabase.ExecuteBackupPath();
        }
    }
}
