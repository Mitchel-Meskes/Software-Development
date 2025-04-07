namespace App.Models
{
    public class User
    {
        public string Username { get; set; }
        public string PasswordHash { get; set; }

        // Constructor die ervoor zorgt dat Username en PasswordHash altijd ingevuld zijn
        public User(string username, string passwordHash)
        {
            Username = username ?? throw new ArgumentNullException(nameof(username));
            PasswordHash = passwordHash ?? throw new ArgumentNullException(nameof(passwordHash));
        }
    }
}
