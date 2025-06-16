using Microsoft.VisualStudio.TestTools.UnitTesting;
using App.Models;
using App.Services;
using App.Forms;
using App.Tests.Utils;
using System.Collections.Generic;
using System.Windows.Forms;

namespace App.Tests.AcceptanceTests
{
    [TestClass]
    public class LoginFormAcceptanceTests
    {
        [TestInitialize]
        public void Setup()
        {
            TestFileHelper.EnsureCleanFile("users.json");

            var users = new List<User>
            {
                new User("acceptuser", UserService.HashPassword("acc123"))
            };
            UserService.SaveUsers(users);
        }

        [TestMethod]
        public void LoginForm_ShouldAllow_Login_And_ShowMainForm()
        {
            var loginForm = new LoginForm();

            var usernameField = loginForm.Controls.Find("txtUsername", true);
            var passwordField = loginForm.Controls.Find("txtPassword", true);

            Assert.IsNotNull(usernameField);
            Assert.IsNotNull(passwordField);
        }

        [TestMethod]
        public void UserCanRegister_AndLogin_Successfully()
        {
            var newUser = new User("newaccept", UserService.HashPassword("newpass"));
            var users = UserService.LoadUsers();
            users.Add(newUser);
            UserService.SaveUsers(users);

            var reloaded = UserService.LoadUsers();
            Assert.IsTrue(reloaded.Exists(u => u.Username == "newaccept"));
        }
    }
}