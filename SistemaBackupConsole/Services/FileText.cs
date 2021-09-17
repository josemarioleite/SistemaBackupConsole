using SistemaBackupConsole.Models;
using System;
using System.Collections.Generic;
using System.IO;

namespace SistemaBackupConsole.Services
{
    public class FileText
    {
        FileConfiguration fileConfig = new();
        public FileText()
        {
            fileConfig = new();
        }
        public bool VerifyFileExists()
        {
            if (!File.Exists(fileConfig.FileConfig))
            {
                try
                {
                    using (StreamWriter sw = new StreamWriter(fileConfig.FileConfig))
                    {
                        sw.WriteLine("");
                    }
                    fileConfig.FileExists = true;
                } catch (Exception)
                {
                    fileConfig.FileExists = false;
                }                
            } else
            {
                fileConfig.FileExists = true;
            }
            return fileConfig.FileExists;
        }
        public List<string> ReadFile()
        {
            List<string> txt = new();
            if (fileConfig.FileExists == true)
            {
                using (StreamReader sr = new StreamReader(fileConfig.FileConfig))
                {
                    while (!sr.EndOfStream)
                    {
                        txt.Add(sr.ReadLine());
                    }
                }
            }
            else
            {
                Console.Write("Arquivo Inexistente\n");
            }
            return txt;
        }
    }
}
