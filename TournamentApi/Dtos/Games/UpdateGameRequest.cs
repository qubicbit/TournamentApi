namespace TournamentApi.Dtos.Games
{
    public class UpdateGameRequest
    {
        public string Title { get; set; } = string.Empty;
        public DateTime Time { get; set; }
        public int TournamentId { get; set; }
    }
}
