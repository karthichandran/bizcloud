using AutoMapper;
using ReProServices.Application.Common.Mappings;

namespace ReProServices.Application.RemittanceStatusList
{
   public class RemittanceStatusDto : IMapFrom<Domain.Entities.RemittanceStatus>
    {
        public int RemittanceStatusID { get; set; }
        public string RemittanceStatusText { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Domain.Entities.RemittanceStatus, RemittanceStatusDto>();
        }
    }
}
