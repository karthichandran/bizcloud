using System.Collections.Generic;
using ReProServices.Application.SellerProperties;

namespace ReProServices.Application.Property
{
    public class PropertyVM
    {
        public PropertyDto propertyDto { get; set; }
        public ICollection<SellerPropertyDto> sellerProperties {get;set;}
    }
}
