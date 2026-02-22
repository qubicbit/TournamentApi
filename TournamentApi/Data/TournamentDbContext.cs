using Microsoft.EntityFrameworkCore;
using TournamentApi.Models;

namespace TournamentApi.Data
{
    public class TournamentDbContext : DbContext
    {
        public TournamentDbContext(DbContextOptions<TournamentDbContext> options)
            : base(options)
        {
        }

        public DbSet<Tournament> Tournaments => Set<Tournament>();
        public DbSet<Game> Games => Set<Game>();

        // definiera relationer explicit 
        //EF Core hade kunnat lista ut relationen automatiskt, nen det är en bra vana att definiera den explicit i OnModelCreating
        // om man vill styra delete‑beteende och relationer tydligt
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Tournament>()
                .HasMany(t => t.Games)
                .WithOne(g => g.Tournament)
                .HasForeignKey(g => g.TournamentId)
                .OnDelete(DeleteBehavior.Cascade);

            base.OnModelCreating(modelBuilder);
        }
    }
}
