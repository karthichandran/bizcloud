using AutoMapper;
using ReProServices.Application.Common.Mappings;
using ReProServices.Domain.Entities;

namespace ReProServices.Application.Remarks
{
    public class RemarkDto : IMapFrom<Remark>
    {
        
        public int RemarksID { get; set; }
        public string RemarksText { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Remark, RemarkDto>();
        }
    }
}
