using System;
using System.Linq;

namespace ReProServices.Application.Property.Queries
{
    public static class GetPropertiesQueryFilter
    {
        public static IQueryable<Domain.Entities.Property> FilterPropertiesBy(this IQueryable<Domain.Entities.Property> properties,
            PropertyFilter filter)
        {

            IQueryable<Domain.Entities.Property> propertiesList = properties;
            if (!String.IsNullOrEmpty(filter.AddressPremises))
            {
                propertiesList = propertiesList.Where(x => x.AddressPremises.Contains(filter.AddressPremises) );
            }

            return propertiesList;
        }

    }
}

