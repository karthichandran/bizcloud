using System;
using AutoMapper;
using ReProServices.Application.Common.Mappings;

namespace ReProServices.Application.TaxCodes
{
    public class TaxCodesDto : IMapFrom<Domain.Entities.TaxCodes>
    {
        public int TaxCodeId { get; set; }
        public string TaxName { get; set; }
        public DateTime EffectiveStartDate { get; set; }
        public DateTime EffectiveEndDate { get; set; }
        public decimal TaxValue { get; set; }
        public int? TaxTypeId { get; set; }
        public string TaxTypeDesc { get; set; }
        public string TaxLabel { get; set; }

        public int TaxID { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Domain.Entities.TaxCodes, TaxCodesDto>()
                .ForMember(dest => dest.TaxLabel, opt => opt.MapFrom(src => src.TaxName + '@' + src.TaxValue));
        }

    }
}
