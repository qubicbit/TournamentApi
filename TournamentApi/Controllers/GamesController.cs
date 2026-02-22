using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using TournamentApi.Dtos.Games;
using TournamentApi.Models;
using TournamentApi.Services.Interfaces;

namespace TournamentApi.Controllers;

// Controller = ansvarig för att ta emot HTTP-förfrågningar och returnera HTTP-svar.
// Den pratar INTE direkt med databasen, utan använder GameService.
// AutoMapper används för att mappa mellan entiteter (Game) och DTOs (GameResponse).
// async + AutoMapper + EF Core

[ApiController]
[Route("api/games")]
public class GameController : ControllerBase
{
    private readonly IGameService _gameService;
    private readonly IMapper _mapper;

    // Dependency Injection av service + mapper
    public GameController(IGameService gameService, IMapper mapper)
    {
        _gameService = gameService;
        _mapper = mapper;
    }

    // GET: api/games?search=abc
    // Hämtar alla games, eller flera games specifikt/delar av game om search-query används
    [HttpGet]
    public async Task<ActionResult<IEnumerable<GameResponse>>> Get([FromQuery] string? search)
    {
        if (!string.IsNullOrWhiteSpace(search))
        {
            var games = await _gameService.GetByTitleAsync(search);

            if (games == null || !games.Any())
                return NotFound();

            var response = _mapper.Map<IEnumerable<GameResponse>>(games);
            return Ok(response);
        }

        var allgames = await _gameService.GetAllAsync();
        var responseList = _mapper.Map<IEnumerable<GameResponse>>(allgames);

        return Ok(responseList);
    }

    // GET: api/games/5
    // Hämtar ett game baserat på ID
    [HttpGet("{id:int}")]
    public async Task<ActionResult<GameResponse>> Get(int id)
    {
        var game = await _gameService.GetByIdAsync(id);

        if (game == null)
            return NotFound();

        var response = _mapper.Map<GameResponse>(game);
        return Ok(response);
    }


    [HttpPost]
    [EnableRateLimiting("PostLimit")]
    public async Task<ActionResult<GameResponse>> Create(CreateGameRequest request)
    {
        // Mappa inkommande request → entitet
        var entity = _mapper.Map<Game>(request);

        // Skicka entiteten till servicen för att sparas i databasen
        var created = await _gameService.CreateAsync(entity);

        // Mappa tillbaka entitet → response-DTO
        var response = _mapper.Map<GameResponse>(created);

        return Created($"/api/games/{created.Id}", response);
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult> Update(int id, UpdateGameRequest request)
    {
        // Mappa request → entitet
        var entity = _mapper.Map<Game>(request);
        entity.Id = id;

        var updated = await _gameService.UpdateAsync(id, entity);

        if (updated == null)
            return NotFound();

        return Ok(_mapper.Map<GameResponse>(updated));
    }

    [HttpPatch("{id:int}")]
    public async Task<ActionResult> Patch(int id, PatchGameRequest request)
    {
        var existing = await _gameService.GetByIdAsync(id);
        if (existing == null)
            return NotFound();

        // Mappa inkommande fält → befintlig entitet
        _mapper.Map(request, existing);

        // Spara
        var updated = await _gameService.PatchAsync(id, existing);

        return Ok(_mapper.Map<GameResponse>(updated));
    }



    [HttpDelete("{id:int}")]
    [Authorize]
    public async Task<ActionResult> Delete(int id)
    {
        var deleted = await _gameService.DeleteAsync(id);

        if (!deleted)
            return NotFound();

        return NoContent();
    }
}
