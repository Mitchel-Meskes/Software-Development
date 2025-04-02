using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using App.Models;

namespace App.Services
{
    public static class UserService
    {
        private static readonly string FilePath = "Resources/users.json";

        public static List<User> LoadUsers()
        {
            if (!File.Exists(FilePath)) return new List<User>();
            string json = File.ReadAllText(FilePath);
            return JsonSerializer.Deserialize<List<User>>(json);
        }

        public static void SaveUsers(List<User> users)
        {
            string json = JsonSerializer.Serialize(users, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(FilePath, json);
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
