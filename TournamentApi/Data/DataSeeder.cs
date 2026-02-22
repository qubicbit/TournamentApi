using Microsoft.EntityFrameworkCore;
using TournamentApi.Models;
using TournamentApi.Data;

public static class DataSeeder
{
    public static void Seed(WebApplication app)
    {
        using var scope = app.Services.CreateScope();

        var db = scope.ServiceProvider.GetRequiredService<TournamentDbContext>();

        // skapa databas, utan att skriva Add-Migration InitialCreate och update-Database i pmc .
        // men kommentera bort det (+ radera databasen sedan kör migration) om man ska jobba i riktig projekt i teams.
        //db.Database.EnsureCreated();

        // Töm tabeller
        //db.Games.RemoveRange(db.Games);
        //db.Tournaments.RemoveRange(db.Tournaments);
        //db.SaveChanges();

        // Nollställ ID-räknare
        //db.Database.ExecuteSqlRaw("DBCC CHECKIDENT ('Games', RESEED, 0)");
        //db.Database.ExecuteSqlRaw("DBCC CHECKIDENT ('Tournaments', RESEED, 0)");

        // Kör seedning bara om det inte finns några turneringar
        if (db.Tournaments.Any())
            return;

        // ============================
        //   TURNERINGAR (2 st)
        // ============================

        var championsLeague = new Tournament
        {
            Title = "UEFA Champions League",
            Description = "Europe's top football clubs compete for the ultimate trophy.",
            MaxPlayers = 32 * 25, // 32 lag, ca 25 spelare per lag
            Date = new DateTime(2025, 2, 10)
        };

        var worldCup = new Tournament
        {
            Title = "FIFA World Cup",
            Description = "48 national teams battle for global glory.",
            MaxPlayers = 48 * 26, // 48 landslag, ca 26 spelare per lag
            Date = new DateTime(2026, 6, 8)
        };

        var AsianCup = new Tournament
        {
            Title = "AFC Asian Cup",
            Description = "Asia's top national teams compete for continental glory.",
            MaxPlayers = 25 * 26, // 25 landslag, ca 26 spelare per lag
            Date = new DateTime(2026, 6, 8)
        };

        var CopaLibertadores = new Tournament
        {
            Title = "Copa Libertadores",
            Description = "South America's most prestigious club tournament, known for intense rivalries.",
            MaxPlayers = 30 * 26, // 30 lag, ca 26 spelare per lag
            Date = new DateTime(2026, 10, 8)
        };


        db.Tournaments.AddRange(championsLeague, worldCup, AsianCup, CopaLibertadores);
        db.SaveChanges();

        // ============================
        //   MATCHER (Games)
        // ============================

        db.Games.AddRange(
            // Champions League
            new Game
            {
                Title = "Barcelona vs Bayern Munich",
                Time = new DateTime(2025, 2, 15, 20, 45, 00),
                TournamentId = championsLeague.Id
            },
            new Game
            {
                Title = "Real Madrid vs Liverpool",
                Time = new DateTime(2025, 2, 16, 20, 45, 00),
                TournamentId = championsLeague.Id
            },

            // World Cup
            new Game
            {
                Title = "Opening Match: USA vs Mexico",
                Time = new DateTime(2026, 6, 8, 18, 00, 00),
                TournamentId = worldCup.Id
            },
            new Game
            {
                Title = "Brazil vs Argentina",
                Time = new DateTime(2026, 6, 10, 21, 00, 00),
                TournamentId = worldCup.Id
            }


        );

        db.SaveChanges();
    }
}
