using AutoMapper;
using TournamentApi.Dtos.Tournaments;
using TournamentApi.Models;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace TournamentApi.Profiles
{
    public class TournamentProfile : Profile
    {
        public TournamentProfile()
        {
            // Create
            CreateMap<CreateTournamentRequest, Tournament>();

            // Update
            CreateMap<UpdateTournamentRequest, Tournament>();

            // Response
            CreateMap<Tournament, TournamentResponse>();
        }
    }
}
