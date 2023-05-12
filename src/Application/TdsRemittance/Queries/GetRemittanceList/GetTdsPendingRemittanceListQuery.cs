using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ReProServices.Application.Common.Interfaces;
using ReProServices.Domain.Enums;

namespace ReProServices.Application.TdsRemittance.Queries.GetRemittanceList
{
    public class GetTdsPendingRemittanceListQuery : IRequest<IList<TdsRemittanceDto>>
    {
        public TdsRemittanceFilter Filter { get; set; }
        public class GetTdsPendingRemittanceListQueryHandler : IRequestHandler<GetTdsPendingRemittanceListQuery, IList<TdsRemittanceDto>>
        {
            private readonly IApplicationDbContext _context;

            public GetTdsPendingRemittanceListQueryHandler(IApplicationDbContext context)
            {
                _context = context;
            }

            public async Task<IList<TdsRemittanceDto>> Handle(GetTdsPendingRemittanceListQuery request, CancellationToken cancellationToken)
            {
                var remittances =  (from pay in _context.ClientPayment
                                         join cpt in _context.ClientPaymentTransaction on pay.ClientPaymentID equals cpt.ClientPaymentID
                                         join cp in _context.ViewCustomerPropertyExpanded on new { cpt.OwnershipID, cpt.CustomerID } equals new { cp.OwnershipID, cp.CustomerID }
                                         join sp in _context.ViewSellerPropertyExpanded on cp.PropertyID equals sp.PropertyID
                                    join da in _context.DebitAdvices on cpt.ClientPaymentTransactionID equals da.ClientPaymentTransactionID into xObj
                                    from dam in xObj.DefaultIfEmpty()
                                    where cpt.RemittanceStatusID == (int)ERemittanceStatus.Pending
                                               && pay.NatureOfPaymentID == (int)ENatureOfPayment.ToBeConsidered
                                               && cpt.SellerID == sp.SellerID && cp.StatusTypeID!=3
                                         select new TdsRemittanceDto
                                         {
                                             ClientPaymentTransactionID = cpt.ClientPaymentTransactionID,
                                             CustomerName = cp.CustomerName,
                                             CustomerShare = cpt.CustomerShare,
                                             SellerName = sp.SellerName,
                                             SellerShare = cpt.SellerShare,
                                             SellerPAN=sp.SellerPAN,
                                             PropertyPremises = sp.PropertyPremises,
                                             UnitNo = cp.UnitNo,
                                             TdsCollectedBySeller = cp.TdsCollectedBySeller,
                                             OwnershipID = cp.OwnershipID,
                                             InstallmentID = pay.InstallmentID,
                                             StatusTypeID = cp.StatusTypeID,
                                             GstAmount = cpt.Gst,
                                             TdsInterest = cpt.TdsInterest,
                                             AmountPaid = pay.AmountPaid,
                                             GrossAmount = pay.GrossAmount,
                                             RevisedDateOfPayment = pay.RevisedDateOfPayment,
                                             DateOfDeduction = pay.DateOfDeduction,
                                             ReceiptNo = pay.ReceiptNo,
                                             LateFee = cpt.LateFee,
                                             ClientPaymentID = pay.ClientPaymentID,
                                             LotNo = pay.LotNo,
                                             GrossShareAmount = cpt.GrossShareAmount,
                                             TdsAmount = cpt.Tds,
                                             AmountShare = cpt.ShareAmount,
                                             RemittanceStatusID = cpt.RemittanceStatusID,
                                             IsDebitAdvice=dam!=null?true:false
                                         })
                    .PreFilterRemittanceBy(request.Filter)
                    .ToList()
                    .AsQueryable()
                    .PostFilterRemittanceBy(request.Filter)
                    .ToList();
                return remittances;
            }

        }
    }
}
