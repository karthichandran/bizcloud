using AutoMapper;
using ReProServices.Application.Common.Mappings;
using ReProServices.Domain.Entities;

namespace ReProServices.Application.SellerProperties
{
    public class SellerPropertyDto : IMapFrom<SellerProperty>
    {
        public int SellerPropertyId { get; set; }
        public decimal SellerShare { get; set; }
        public int PropertyID { get; set; }
        public virtual string AddressPremises { get; set; }
        public int SellerID { get; set; }
        public bool? IsSharedSeller { get; set; }
        public virtual string SellerName { get; set; }
        public virtual string SellerPAN { get; set; }

        public virtual bool MarkForDelete { get; set; } = false;
        public void Mapping(Profile profile)
        {
            profile.CreateMap<SellerProperty, SellerPropertyDto>();
        }
    }
}
