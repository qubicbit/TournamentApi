using TournamentApi.Models;

namespace TournamentApi.Services.Interfaces
{
    public interface ITournamentService
    {
        Task<IEnumerable<Tournament>> GetAllAsync();
        Task<Tournament?> GetByIdAsync(int id);
        //Task<Tournament?> GetByTitleAsync(string title);
        Task<IEnumerable<Tournament?>> GetByTitleAsync(string search);
        Task<Tournament> CreateAsync(Tournament tournament);
        Task<Tournament?> UpdateAsync(int id, Tournament tournament);
        Task<bool> DeleteAsync(int id);
    }
}
