using Microsoft.VisualStudio.TestTools.UnitTesting;
using App.Services;
using App.Models;
using System.Collections.Generic;
using System.IO;

namespace App.Tests.Services
{
    [TestClass]
    public class UserServiceTests
    {
        [TestInitialize]
        public void Cleanup()
        {
            if (File.Exists("users.json"))
                File.Delete("users.json");
        }

        [TestMethod]
        public void HashPassword_ShouldReturnHashedValue()
        {
            string password = "test123";
            string hashed = UserService.HashPassword(password);

            Assert.IsFalse(string.IsNullOrWhiteSpace(hashed));
            Assert.AreNotEqual(password, hashed);
        }

        [TestMethod]
        public void SaveAndLoadUser_ShouldPersistData()
        {
            var users = new List<User>
            {
                new User("unituser", UserService.HashPassword("unit123"))
            };

            UserService.SaveUsers(users);
            var loaded = UserService.LoadUsers();

            Assert.IsTrue(loaded.Exists(u => u.Username == "unituser"));
        }
    }
}
