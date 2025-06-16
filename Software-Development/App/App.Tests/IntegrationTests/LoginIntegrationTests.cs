using Microsoft.VisualStudio.TestTools.UnitTesting;
using App.Models;
using App.Services;
using App.Tests.Utils;
using System.Collections.Generic;

namespace App.Tests.IntegrationTests
{
    [TestClass]
    public class LoginIntegrationTests
    {
        [TestInitialize]
        public void Setup()
        {
            TestFileHelper.EnsureCleanFile("users.json");
            TestFileHelper.EnsureCleanFile("statistics.json");

            var users = new List<User>
            {
                new User("loginuser", UserService.HashPassword("login123")),
                new User("tester", UserService.HashPassword("abc123"))
            };

            UserService.SaveUsers(users);
        }

        [TestMethod]
        public void Login_ShouldSucceed_WithValidCredentials()
        {
            string username = "loginuser";
            string password = "login123";
            string hash = UserService.HashPassword(password);

            var users = UserService.LoadUsers();
            var match = users.Find(u => u.Username == username && u.PasswordHash == hash);

            Assert.IsNotNull(match);
        }

        [TestMethod]
        public void GameStats_ShouldReturnData_ForExistingUser()
        {
            var stats = new List<GameStats>
            {
                new GameStats("tester", 3, 45.0)
            };

            StatisticsService.SaveStats(stats);
            var loaded = StatisticsService.LoadStats();

            Assert.IsTrue(loaded.Exists(s => s.Username == "tester"));
        }
    }
}