using AutoMapper;
using ReProServices.Application.Common.Mappings;
using ReProServices.Domain.Entities;


namespace ReProServices.Application.TaxTypes.Queries
{
    public class TaxTypeDto : IMapFrom<TaxType>
    {
        public int TaxTypeId { get; set; }
        public string TaxTypeDesc { get; set; }
        public void Mapping(Profile profile)
        {
            profile.CreateMap<TaxType, TaxTypeDto>();
        }
    }
}
