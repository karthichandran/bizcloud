using System.Linq;

namespace ReProServices.Application.TaxCodes.Queries.GetTaxCodes
{
    public static class GetTaxCodesQueryFilter
    {
        public static IQueryable<Domain.Entities.TaxCodes> FilterTaxCodesBy(this IQueryable<Domain.Entities.TaxCodes> taxCodes,
            TaxCodesFilter filter)
        {
            //filter.SellerName = "Pur";
            IQueryable<Domain.Entities.TaxCodes> taxCodesList = taxCodes;

            if (!string.IsNullOrEmpty(filter.TaxName))
            {
                taxCodesList = taxCodesList.Where(x => x.TaxName.Contains(filter.TaxName));
            }

            if (!(filter.TaxType is null))
            {
                taxCodesList = taxCodesList.Where(x => x.TaxTypeId == (int)filter.TaxType);
            }

            return taxCodesList;
        }

    }
}

