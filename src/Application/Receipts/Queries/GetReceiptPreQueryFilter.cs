using ReProServices.Application.Property.Queries;
using ReProServices.Domain.Entities;
using System;
using System.Linq;

namespace ReProServices.Application.Receipts.Queries
{
    public static class GetReceiptPreQueryFilter
    {
        public static IQueryable<ReceiptDto> PreFilterReceiptsBy(this IQueryable<ReceiptDto> receipts,
            ReceiptFilter filter)
        {
            IQueryable<ReceiptDto> receiptsList = receipts;
            if (filter.PropertyID > 0)
            {
                receiptsList = receiptsList.Where(x => x.PropertyID == filter.PropertyID);
            }
            if (filter.UnitNo > 0)
            {
                receiptsList = receiptsList.Where(x => x.UnitNo == filter.UnitNo);
            }
            if (filter.LotNo > 0)
            {
                receiptsList = receiptsList.Where(x => x.LotNo == filter.LotNo);
            }
            return receiptsList;
        }

    }
}

