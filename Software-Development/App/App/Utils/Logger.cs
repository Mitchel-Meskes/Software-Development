using System;
using System.IO;

namespace App.Utils
{
    public static class Logger
    {
        private static readonly string LogFilePath = "Resources/log.txt";

        public static void Info(string message) => Write("INFO", message);
        public static void Debug(string message) => Write("DEBUG", message);
        public static void Warning(string message) => Write("WARNING", message);
        public static void Error(string message) => Write("ERROR", message);

        private static void Write(string level, string message)
        {
            var log = $"{DateTime.Now:u} [{level}] {message}";
            File.AppendAllText(LogFilePath, log + Environment.NewLine);
        }
    }
}
