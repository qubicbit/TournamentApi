using Microsoft.EntityFrameworkCore;
using System;
using TournamentApi.Data;
using TournamentApi.Services.Interfaces;
using TournamentApi.Models;

namespace TournamentApi.Services
{
    public class TournamentService : ITournamentService
    {
        private readonly TournamentDbContext _context;

        public TournamentService(TournamentDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Tournament>> GetAllAsync()
        {
            return await _context.Tournaments
                .Include(t => t.Games)
                .ToListAsync();
        }

        public async Task<Tournament?> GetByIdAsync(int id)
        {
            return await _context.Tournaments
                .Include(t => t.Games)
                .FirstOrDefaultAsync(t => t.Id == id);
        }

        //public async Task<Tournament?> GetByTitleAsync(string title)
        //{
        //    return await _context.Tournaments
        //        .Include(t => t.Games)
        //        .FirstOrDefaultAsync(t => t.Title == title);
        //}

        public async Task<IEnumerable<Tournament?>> GetByTitleAsync(string search)
        {
            return await _context.Tournaments
                .Include(t => t.Games)
                .Where(t => t.Title.ToLower().Contains(search.ToLower()))
                .ToListAsync();
        }

        public async Task<Tournament> CreateAsync(Tournament tournament)
        {
            await _context.Tournaments.AddAsync(tournament);
            await _context.SaveChangesAsync();

            return tournament;
        }

        public async Task<Tournament?> UpdateAsync(int id, Tournament updated)
        {
            if (updated.Id != id)
                return null;

            var existing = await _context.Tournaments.FirstOrDefaultAsync(t => t.Id == id);
            if (existing == null)
                return null;

            existing.Title = updated.Title;
            existing.Description = updated.Description;
            existing.Date = updated.Date;
            existing.MaxPlayers = updated.MaxPlayers;

            await _context.SaveChangesAsync();
            return existing;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var tournament = await _context.Tournaments.FirstOrDefaultAsync(t => t.Id == id);
            if (tournament == null)
                return false;

            _context.Tournaments.Remove(tournament);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
