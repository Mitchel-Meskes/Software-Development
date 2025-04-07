namespace App.Models
{
    public class GameStats
    {
        public string Username { get; set; }
        public int CompletedPuzzles { get; set; }
        public double TotalPlayTimeMinutes { get; set; }

        // Constructor die ervoor zorgt dat Username altijd ingevuld is
        public GameStats(string username, int completedPuzzles, double totalPlayTimeMinutes)
        {
            Username = username ?? throw new ArgumentNullException(nameof(username));
            CompletedPuzzles = completedPuzzles;
            TotalPlayTimeMinutes = totalPlayTimeMinutes;
        }
    }
}
