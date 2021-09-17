using SistemaBackupConsole.Models;
using System;
using System.Collections.Generic;
using System.IO;

namespace SistemaBackupConsole.Services
{
    public class Logger
    {
        public static void BreakLine()
        {
            string file = Path.Combine(Constant.PathFile, Constant.NameFile);
            if (!File.Exists(file))
            {
                FileStream arquivo = File.Create(file);
                arquivo.Close();
            }
            using (StreamWriter w = File.AppendText(file))
            {
                AppendBreakLine("\n\n", w);
            }
        }
        public static bool Log(string strMensagem, Exception ex)
        {
            try
            {
                string file = Path.Combine(Constant.PathFile, Constant.NameFile);
                if (!File.Exists(file))
                {
                    FileStream arquivo = File.Create(file);
                    arquivo.Close();
                }
                using (StreamWriter w = File.AppendText(file))
                {
                    AppendLog(strMensagem, w, ex);
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public static bool Log(string strMensagem)
        {
            try
            {
                string file = Path.Combine(Constant.PathFile, Constant.NameFile);
                if (!File.Exists(file))
                {
                    FileStream arquivo = File.Create(file);
                    arquivo.Close();
                }
                using (StreamWriter w = File.AppendText(file))
                {
                    AppendLog(strMensagem, w);
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        private static void AppendBreakLine(string logMensagem, TextWriter txtWriter)
        {
            try
            {
                txtWriter.Write("...................................................................................");
                txtWriter.WriteLine($"{logMensagem}");
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        private static void AppendLog(string logMensagem, TextWriter txtWriter)
        {
            try
            {
                txtWriter.Write("...................................................................................");
                txtWriter.Write("\r\nData e Hora: ");
                txtWriter.WriteLine($"{DateTime.Now.ToLongDateString()} - {DateTime.Now.ToLongTimeString()}");
                txtWriter.Write("Text: ");
                txtWriter.WriteLine($"{logMensagem}");
            }
            catch (Exception e)
            {
                throw;
            }
        }
        private static void AppendLog(string logMensagem, TextWriter txtWriter, Exception ex)
        {
            try
            {
                txtWriter.Write("\n...................................................................................");
                txtWriter.Write("\r\nData e Hora: ");
                txtWriter.WriteLine($"{DateTime.Now.ToLongDateString()} - {DateTime.Now.ToLongTimeString()}");
                txtWriter.Write("Text: ");
                txtWriter.WriteLine($"{logMensagem}");
                txtWriter.Write("Erro (Exception Message): ");
                txtWriter.WriteLine($"{ex.Message}");
                txtWriter.Write("Erro (Inner Exception): ");
                txtWriter.WriteLine($"{ex.InnerException}");
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}
