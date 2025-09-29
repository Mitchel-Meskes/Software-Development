namespace App.Models
{
    public class User
    {
        public string Username { get; set; }
        public string PasswordHash { get; set; }

        public User(string username, string passwordHash)
        {
            Username = username ?? throw new ArgumentNullException(nameof(username));
            PasswordHash = passwordHash ?? throw new ArgumentNullException(nameof(passwordHash));
        }
    }
}
