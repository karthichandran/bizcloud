using ReProServices.Domain.Entities;
using System;
using System.Linq;

namespace ReProServices.Application.Sellers.Queries.GetSellers
{
    public static class GetSellersQueryFilter
    {
        public static IQueryable<Seller> FilterSellersBy(this IQueryable<Seller> sellers,
            SellerFilter filter)
        {
            //filter.SellerName = "Pur";


            IQueryable<Seller> sellerList = sellers;
            if (!String.IsNullOrEmpty(filter.MobileNo))
            {
                sellerList = sellerList.Where(x => x.MobileNo == filter.MobileNo);
            }

            if (!String.IsNullOrEmpty(filter.PAN))
            {
                sellerList = sellerList.Where(x => x.PAN == filter.PAN);
            }

            if (!String.IsNullOrEmpty(filter.SellerName))
            {
                sellerList = sellerList.Where(x => x.SellerName.Contains(filter.SellerName));
            }

            return sellerList;
        }

    }
}

