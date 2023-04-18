using System;
using System.Linq;
using ReProServices.Domain.Entities;

namespace ReProServices.Application.SellerProperties.Queries
{
    public static class GetSellerPropertyQueryFilter
    {
        public static IQueryable<ViewSellerPropertyBasic> FilterSellerPropertiesBy(this IQueryable<ViewSellerPropertyBasic> properties,
            SellerPropertyFilter filter)
        {

            IQueryable<ViewSellerPropertyBasic> propertiesList = properties;
            if (!String.IsNullOrEmpty(filter.AddressPremises))
            {
                propertiesList = propertiesList.Where(x => x.PropertyPremises.Contains(filter.AddressPremises) );
            }
            if (!String.IsNullOrEmpty(filter.SellerName))
            {
                propertiesList = propertiesList.Where(x => x.SellerName.Contains(filter.SellerName));
            }
            if (!String.IsNullOrEmpty(filter.PAN))
            {
                propertiesList = propertiesList.Where(x => x.SellerPAN == filter.PAN);
            }
            if (filter.PropertyId > 0)
            {
                propertiesList = propertiesList.Where(x => x.PropertyID == filter.PropertyId);
            }

            return propertiesList;
        }

    }
}

