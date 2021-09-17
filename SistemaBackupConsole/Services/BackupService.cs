using MySql.Data.MySqlClient;
using SistemaBackupConsole.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Threading.Tasks;

namespace SistemaBackupConsole.Services
{
    public class BackupService
    {
        FileText fileText = new();
        public BackupService()
        {
            VerifySizeFile();
            fileText = new();
        }
        public void VerifySizeFile()
        {
            string file = Path.Combine(Constant.PathFile, Constant.NameFile);
            List<string> txt = new();
            if (File.Exists(file))
            {
                using (StreamReader sr = new StreamReader(file))
                {
                    while (!sr.EndOfStream)
                    {
                        txt.Add(sr.ReadLine());
                    }
                }
                if (txt.Count > 1000)
                {
                    File.Delete(file);
                }
            }            
        }
        public string ExecuteBackupDatabase()
        {
            Console.WriteLine("Executando Backup do banco de dados...");
            fileText.VerifyFileExists();
            var fileExists = fileText.ReadFile();
            string returnFileDatabase = "";
            if (fileExists.Count > 0)
            {
                try
                {
                    string conexion = ConfigurationManager.ConnectionStrings["conectDatabase"].ConnectionString;
                    string savePath = fileExists[0].Replace("pasta para backup: ", "");
                    string nameSQL = "data.sql";
                    if (File.Exists(Path.Combine(savePath, nameSQL)))
                    {
                        File.Delete(Path.Combine(savePath, nameSQL));
                    }
                    string nameFileSql = $"bkpDatabase{DateTime.Now.ToString("dd-MM-yyyy")}_{DateTime.Now.ToString("HH")}-{DateTime.Now.ToString("mm")}-" + nameSQL;
                    string file = Path.Combine(savePath, nameFileSql);
                    using (MySqlConnection conn = new(conexion))
                    {
                        using (MySqlCommand cmd = new())
                        {
                            using (MySqlBackup bkp = new(cmd))
                            {
                                cmd.Connection = conn;
                                conn.Open();
                                bkp.ExportToFile(file);
                                conn.Close();
                                returnFileDatabase = Path.Combine(savePath, nameFileSql);
                                string msg = "Backup do banco realizado com sucesso!!!";
                                Console.WriteLine(msg);
                                Logger.Log(msg);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("erro ao exportar");
                    Logger.Log("Escrevendo Log...", ex);
                }
            } else
            {
                Console.WriteLine("Não há dados no arquivo");
                Logger.Log("Arquivo não configurado");
            }
            return returnFileDatabase;
        }
        public void ExecuteBackupPath()
        {
            Console.WriteLine("Executando Backup da pasta...");
            var fileExists = fileText.ReadFile();
            if (fileExists.Count > 0)
            {
                string pathToSave = fileExists[0].Replace("pasta para backup: ", "");
                string nameFile = $"bkp{DateTime.Now.ToString("dd-MM-yyyy")}_{DateTime.Now.ToString("HH")}-{DateTime.Now.ToString("mm")}" + ".zip";
                string savePath = fileExists[1].Replace("salvar em: ", "");
                string zipFILE = Path.Combine(savePath, nameFile);
                if (File.Exists(zipFILE))
                {
                    File.Delete(zipFILE);
                }
                try
                {
                    Task.Run(() => ZipFile.CreateFromDirectory(pathToSave, zipFILE)).Wait(TimeSpan.FromSeconds(10));
                    string msg = "Backup da pasta realizado com sucesso";
                    Console.WriteLine(msg);
                    Logger.Log(msg);
                } catch (Exception ex)
                {
                    string msg = "Não foi possível realizar backup da pasta";
                    Console.WriteLine(msg);
                    Logger.Log(msg, ex);
                }
            }
            Logger.BreakLine();
        }
        public List<FileInfo> GetFiles(string path, params string[] extensions)
        {
            List<FileInfo> list = new List<FileInfo>();
            foreach (string ext in extensions)
            {
                list.AddRange(new DirectoryInfo(path).GetFiles("*" + ext).Where(p => p.Extension.Equals(ext, StringComparison.CurrentCultureIgnoreCase)).ToArray());
            }
            return list;
        }
    }
}
