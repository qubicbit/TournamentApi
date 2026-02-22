using TournamentApi.Models;

namespace TournamentApi.Services.Interfaces
{
    public interface IGameService
    {
        Task<IEnumerable<Game>> GetAllAsync();
        Task<Game?> GetByIdAsync(int id);

        //Task<Game?> GetByTitleAsync(string title);
        Task<IEnumerable<Game?>> GetByTitleAsync(string search);
        Task<Game> CreateAsync(Game game);
        Task<Game?> UpdateAsync(int id, Game game);
        Task<Game?> PatchAsync(int id, Game game);
        Task<bool> DeleteAsync(int id);
    }
}
