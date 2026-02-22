namespace TournamentApi.Models
{
    public class Tournament
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int MaxPlayers { get; set; }
        public DateTime Date { get; set; }

        // relation: 1‑till‑många 
        //En Tournament → många Games

        // Navigation property (1 -> many)
        public ICollection<Game> Games { get; set; } = new List<Game>();
    }
}
