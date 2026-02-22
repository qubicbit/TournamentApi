using Microsoft.EntityFrameworkCore;
using TournamentApi.Data;
using TournamentApi.Models;
using TournamentApi.Services.Interfaces;

namespace TournamentApi.Services
{
    public class GameService : IGameService
    {
        private readonly TournamentDbContext _context;

        public GameService(TournamentDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Game>> GetAllAsync()
        {
            return await _context.Games
                //“När du hämtar Games, gör en JOIN och hämta även Tournament‑objektet
                .Include(g => g.Tournament)
                .ToListAsync();
        }

        public async Task<Game?> GetByIdAsync(int id)
        {
            return await _context.Games
                .Include(g => g.Tournament)
                .FirstOrDefaultAsync(g => g.Id == id);
        }

        public async Task<IEnumerable<Game?>> GetByTitleAsync(string search)
        {
            return await _context.Games
                .Include(g => g.Tournament)
                .Where(g => g.Title.ToLower().Contains(search.ToLower()))
                .ToListAsync();
            //.FirstOrDefaultAsync(g => g.Title == title);
        }

        public async Task<Game> CreateAsync(Game game)
        {
            await _context.Games.AddAsync(game);
            await _context.SaveChangesAsync();
            return game;
        }

        public async Task<Game?> UpdateAsync(int id, Game updated)
        {
            if (updated.Id != id)
                return null;

            var existing = await _context.Games.FirstOrDefaultAsync(g => g.Id == id);
            if (existing == null)
                return null;

            existing.Title = updated.Title;
            existing.Time = updated.Time;
            existing.TournamentId = updated.TournamentId;

            await _context.SaveChangesAsync();
            return existing;
        }

        public async Task<Game?> PatchAsync(int id, Game patchedGame)
        {
            // EF spårar patchedGame eftersom det kom från controllern
            await _context.SaveChangesAsync();

            // Ladda om med Tournament inkluderad
            return await _context.Games
                .Include(g => g.Tournament)
                .FirstOrDefaultAsync(g => g.Id == id);
        }


        public async Task<bool> DeleteAsync(int id)
        {
            var game = await _context.Games.FirstOrDefaultAsync(g => g.Id == id);
            if (game == null)
                return false;

            _context.Games.Remove(game);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
