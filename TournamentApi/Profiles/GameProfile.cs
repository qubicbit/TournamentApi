using AutoMapper;
using TournamentApi.Dtos.Games;
using TournamentApi.Models;

namespace TournamentApi.Profiles
{
    public class GameProfile : Profile
    {
        public GameProfile()
        {
            // Create
            CreateMap<CreateGameRequest, Game>();

            // Update
            CreateMap<UpdateGameRequest, Game>();

            // Patch
            CreateMap<PatchGameRequest, Game>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcValue) => srcValue != null));

            // Response
            CreateMap<Game, GameResponse>();
        }
    }
}
