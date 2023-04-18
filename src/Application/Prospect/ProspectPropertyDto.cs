using AutoMapper;
using ReProServices.Application.Common.Mappings;
using System;

namespace ReProServices.Application.Prospect
{
    public class ProspectPropertyDto: IMapFrom<Domain.Entities.ProspectProperty>
    {
        public int ProspectPropertyID { get; set; }
        public int PropertyID { get; set; }
        public int UnitNo { get; set; }
        public DateTime DeclarationDate { get; set; }
        public Guid? OwnershipID { get; set; }
        public void Mapping(Profile profile)
        {
            profile.CreateMap<Domain.Entities.ProspectProperty, ProspectPropertyDto>();
        }
    }
}
