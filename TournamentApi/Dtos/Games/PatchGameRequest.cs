namespace TournamentApi.Dtos.Games
{
    public class PatchGameRequest
    {
        public string? Title { get; set; }
        public DateTime? Time { get; set; }
        public int? TournamentId { get; set; }
    }
}
