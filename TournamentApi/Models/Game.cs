namespace TournamentApi.Models
{
    public class Game
    {
        public int Id { get; set; }

        public string Title { get; set; } = string.Empty;

        public DateTime Time { get; set; }

        // Foreign key
        public int TournamentId { get; set; }

        // Navigation property
        public Tournament Tournament { get; set; } = null!;
    }
}
