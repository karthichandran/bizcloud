using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ReProServices.Application.Common.Interfaces;
using ReProServices.Application.TdsRemittance;
using ReProServices.Application.TdsRemittance.Queries;
using ReProServices.Application.TdsRemittance.Queries.GetRemittanceList;
using ReProServices.Domain.Enums;

namespace ReProServices.Application.Traces
{
    public class GetTracesListQuery : IRequest<IList<TdsRemittanceDto>>
    {
        public TdsRemittanceFilter Filter { get; set; }
        public class GetTracesListQueryHandler :
                              IRequestHandler<GetTracesListQuery, IList<TdsRemittanceDto>>
        {

            private readonly IApplicationDbContext _context;

            public GetTracesListQueryHandler(IApplicationDbContext context)
            {
                _context = context;
            }

            public async Task<IList<TdsRemittanceDto>> Handle(GetTracesListQuery request, CancellationToken cancellationToken)
            {

                var remittances = (from pay in _context.ClientPayment
                                         join cpt in _context.ClientPaymentTransaction on pay.ClientPaymentID equals cpt.ClientPaymentID
                                         join cp in _context.ViewCustomerPropertyExpanded on new { cpt.OwnershipID, cpt.CustomerID } equals new { cp.OwnershipID, cp.CustomerID }
                                         join sp in _context.ViewSellerPropertyExpanded on cp.PropertyID equals sp.PropertyID
                                         join rem in _context.Remittance on cpt.ClientPaymentTransactionID equals rem.ClientPaymentTransactionID
                                         where cpt.RemittanceStatusID >= (int)ERemittanceStatus.TdsPaid
                                               && cpt.SellerID == sp.SellerID
                                         select new TdsRemittanceDto
                                         {
                                             ClientPaymentTransactionID = cpt.ClientPaymentTransactionID,
                                             CustomerName = cp.CustomerName,
                                             CustomerShare = cpt.CustomerShare,
                                             SellerName = sp.SellerName,
                                             SellerShare = cpt.SellerShare,
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
                                             ChallanDate = rem.ChallanDate,
                                             ChallanAckNo = rem.ChallanAckNo
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
