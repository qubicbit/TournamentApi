using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using TournamentApi.Dtos.Games;
using TournamentApi.Dtos.Tournaments;
using TournamentApi.Models;
using TournamentApi.Services;
using TournamentApi.Services.Interfaces;

namespace TournamentApi.Controllers;

// Controller = ansvarig för att ta emot HTTP-förfrågningar och returnera HTTP-svar.
// Den pratar INTE direkt med databasen, utan använder TournamentService.
// AutoMapper används för att mappa mellan entiteter (Tournament) och DTOs (TournamentResponse).
// async + AutoMapper + EF Core

[ApiController]
[Route("api/tournaments")]
public class TournamentController : ControllerBase
{
    private readonly ITournamentService _tournamentService;
    private readonly IMapper _mapper;

    // Dependency Injection av service + mapper
    public TournamentController(ITournamentService tournamentService, IMapper mapper)
    {
        _tournamentService = tournamentService;
        _mapper = mapper;
    }

    // GET: api/tournaments?search=abc
    // Hämtar alla tournaments, eller ett specifikt tournament om search-query används
    [HttpGet]
    public async Task<ActionResult<IEnumerable<TournamentResponse>>> Get([FromQuery] string? search)
    {
        if (!string.IsNullOrWhiteSpace(search))
        {
            var tournaments = await _tournamentService.GetByTitleAsync(search);

            if (tournaments == null || !tournaments.Any())
                return NotFound();

            var response = _mapper.Map<IEnumerable<TournamentResponse>>(tournaments);
            return Ok(response);
        }

        var alltournaments = await _tournamentService.GetAllAsync();
        var responseList = _mapper.Map<IEnumerable<TournamentResponse>>(alltournaments);

        return Ok(responseList);
    }


    // GET: api/tournaments/5
    // Hämtar ett tournament baserat på ID
    [HttpGet("{id:int}")]
    public async Task<ActionResult<TournamentResponse>> Get(int id)
    {
        var tournament = await _tournamentService.GetByIdAsync(id);

        if (tournament == null)
            return NotFound();

        var response = _mapper.Map<TournamentResponse>(tournament);
        return Ok(response);
    }

    // POST: api/tournaments
    // Skapar ett nytt tournament
    [HttpPost]
    [EnableRateLimiting("PostLimit")]
    public async Task<ActionResult<TournamentResponse>> Create(CreateTournamentRequest request)
    {
        // Mappa inkommande request → entitet
        var entity = _mapper.Map<Tournament>(request);

        // Skicka entiteten till servicen för att sparas i databasen
        var created = await _tournamentService.CreateAsync(entity);

        // Mappa tillbaka entitet → response-DTO
        var response = _mapper.Map<TournamentResponse>(created);

        return Created($"/api/tournaments/{created.Id}", response);
    }

    // PUT: api/tournaments/5
    // Uppdaterar ett befintligt tournament
    [HttpPut("{id:int}")]
    public async Task<ActionResult> Update(int id, UpdateTournamentRequest request)
    {
        // Mappa request → entitet
        var entity = _mapper.Map<Tournament>(request);
        entity.Id = id;

        var updated = await _tournamentService.UpdateAsync(id, entity);

        if (updated == null)
            return NotFound();

        return Ok(_mapper.Map<TournamentResponse>(updated));
    }

    // DELETE: api/tournaments/5
    // Tar bort ett tournament baserat på ID
    [HttpDelete("{id:int}")]
    [Authorize]
    public async Task<ActionResult> Delete(int id)
    {
        var deleted = await _tournamentService.DeleteAsync(id);

        if (!deleted)
            return NotFound();

        return NoContent();
    }
}
