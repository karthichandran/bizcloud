using AutoMapper;
using ReProServices.Application.Common.Mappings;
using ReProServices.Domain.Entities;

namespace ReProServices.Application.Remittance_Remark
{
    public class RemittanceRemarkDto:  IMapFrom<Domain.Entities.RemittanceRemark>
    {
        public int RemarkId { get; set; }

        public string Description { get; set; }
        public bool IsRemittance { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Domain.Entities.RemittanceRemark, RemittanceRemarkDto>();
        }
    }
}
