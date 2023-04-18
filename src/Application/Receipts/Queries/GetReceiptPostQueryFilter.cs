using ReProServices.Application.Property.Queries;
using System;
using System.Linq;

namespace ReProServices.Application.Receipts.Queries
{
    public static class GetReceiptPostQueryFilter
    {
        public static IQueryable<ReceiptDto> PostFilterReceiptsBy(this IQueryable<ReceiptDto> receipts,
            ReceiptFilter filter)
        {
            IQueryable<ReceiptDto> receiptsList = receipts;
            if (!string.IsNullOrEmpty(filter.CustomerName))
            {
                receiptsList = receiptsList.Where(x => x.CustomerName.ToLower().Contains(filter.CustomerName.ToLower()));
            }
           
            return receiptsList;
        }

    }
}

