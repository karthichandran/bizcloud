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
    public class GetRemittanceListQuery : IRequest<IList<TdsRemittanceDto>>
    {
        public TdsRemittanceFilter Filter { get; set; }
        public class GetRemittanceListQueryHandler : IRequestHandler<GetRemittanceListQuery, IList<TdsRemittanceDto>>
        {
            private readonly IApplicationDbContext _context;

            public GetRemittanceListQueryHandler(IApplicationDbContext context)
            {
                _context = context;
            }

            public async Task<IList<TdsRemittanceDto>> Handle(GetRemittanceListQuery request, CancellationToken cancellationToken)
            {
                var remittances = (from pay in _context.ClientPayment
                                         join cpt in _context.ClientPaymentTransaction on pay.ClientPaymentID equals cpt.ClientPaymentID
                                         join cp in _context.ViewCustomerPropertyExpanded on new { cpt.OwnershipID, cpt.CustomerID } equals new { cp.OwnershipID, cp.CustomerID }
                                         join sp in _context.ViewSellerPropertyExpanded on cp.PropertyID equals sp.PropertyID
                                         join rs in _context.RemittanceStatus on cpt.RemittanceStatusID equals rs.RemittanceStatusID
                                         where cpt.RemittanceStatusID >= (int)ERemittanceStatus.Pending
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
                                             RemittanceStatus = rs.RemittanceStatusText,
                                             RemittanceStatusID = cpt.RemittanceStatusID,
                                             
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
