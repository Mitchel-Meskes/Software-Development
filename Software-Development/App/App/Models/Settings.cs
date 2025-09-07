namespace App.Models
{
    public class Settings
    {
        public Preferences Preferences { get; set; }

        public Settings()
        {
            Preferences = new Preferences();
        }
    }

    public class Preferences
    {
        public int GridSize { get; set; } = 5;
        public Difficulty Difficulty { get; set; } = Difficulty.Easy;
    }
}
