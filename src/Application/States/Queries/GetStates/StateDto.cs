using AutoMapper;
using ReProServices.Application.Common.Mappings;

namespace ReProServices.Application.States.Queries.GetStates
{
   public class StateDto : IMapFrom<Domain.Entities.States>
    {
        public int StateID { get; set; }
        public string State { get; set; }
        public string Abbreviation { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Domain.Entities.States, StateDto>();
        }
    }
}
