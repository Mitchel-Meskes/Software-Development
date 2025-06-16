using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using App.Models;
using App.Tests.Utils;
using App.Utils; // TestFileHelper is nu verplaatst naar hoofdproject App.Utils

namespace App.Services
{
    public static class UserService
    {
        private static readonly string FilePath = TestFileHelper.GetTestFilePath("users.json");

        public static List<User> LoadUsers()
        {
            if (!File.Exists(FilePath) || new FileInfo(FilePath).Length == 0)
                return new List<User>();

            string json = File.ReadAllText(FilePath);
            return JsonSerializer.Deserialize<List<User>>(json) ?? new List<User>();
        }

        public static void SaveUsers(List<User> users)
        {
            string json = JsonSerializer.Serialize(users, new JsonSerializerOptions { WriteIndented = true });

            string? dir = Path.GetDirectoryName(FilePath);
            if (!string.IsNullOrEmpty(dir) && !Directory.Exists(dir))
                Directory.CreateDirectory(dir);

            // Forceer veilige overschrijving met file lock-beheer
            const int maxTries = 10;
            for (int i = 0; i < maxTries; i++)
            {
                try
                {
                    using (FileStream fs = new FileStream(FilePath, FileMode.Create, FileAccess.Write, FileShare.None))
                    using (StreamWriter writer = new StreamWriter(fs))
                    {
                        writer.Write(json);
                    }
                    return;
                }
                catch (IOException)
                {
                    Thread.Sleep(100);
                }
                catch (UnauthorizedAccessException)
                {
                    Thread.Sleep(100);
                }
            }

            throw new IOException($"Kon bestand '{FilePath}' niet schrijven na meerdere pogingen.");
        }

        public static string HashPassword(string password)
        {
            using var sha = SHA256.Create();
            byte[] bytes = Encoding.UTF8.GetBytes(password);
            byte[] hash = sha.ComputeHash(bytes);
            return Convert.ToBase64String(hash);
        }
    }
}
