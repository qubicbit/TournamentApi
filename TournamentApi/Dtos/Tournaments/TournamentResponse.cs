namespace TournamentApi.Dtos.Tournaments
{
    public class TournamentResponse
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int MaxPlayers { get; set; }
        public DateTime Date { get; set; }
    }
}
