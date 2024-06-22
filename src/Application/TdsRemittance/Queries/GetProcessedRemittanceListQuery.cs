using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ReProServices.Application.Common.Interfaces;
using ReProServices.Application.TdsRemittance.Queries.GetRemittanceList;
using ReProServices.Domain.Enums;

namespace ReProServices.Application.TdsRemittance.Queries
{
    public class GetProcessedRemittanceListQuery : IRequest<IList<TdsRemittanceDto>>
    {
        public TdsRemittanceFilter Filter { get; set; }
        public class GetProcessedRemittanceListQueryHandler :
                              IRequestHandler<GetProcessedRemittanceListQuery, IList<TdsRemittanceDto>>
        {

            private readonly IApplicationDbContext _context;

            public GetProcessedRemittanceListQueryHandler(IApplicationDbContext context)
            {
                _context = context;
            }

            public async Task<IList<TdsRemittanceDto>> Handle(GetProcessedRemittanceListQuery request, CancellationToken cancellationToken)
            {

                var remittances = (from pay in _context.ClientPayment
                        join cpt in _context.ClientPaymentTransaction on pay.ClientPaymentID equals cpt.ClientPaymentID
                        join cp in _context.ViewCustomerPropertyExpanded on new { cpt.OwnershipID, cpt.CustomerID } equals new { cp.OwnershipID, cp.CustomerID }
                        join sp in _context.ViewSellerPropertyExpanded on cp.PropertyID equals sp.PropertyID
                        join r in _context.Remittance on cpt.ClientPaymentTransactionID equals  r.ClientPaymentTransactionID
                        join remSt in _context.RemittanceStatus on cpt.RemittanceStatusID equals remSt.RemittanceStatusID 
                        where (request.Filter.RemittanceStatusID.HasValue) ? true : cpt.RemittanceStatusID >= (int)ERemittanceStatus.TdsPaid
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
                            GstAmount = cpt.Gst,
                            RevisedDateOfPayment = pay.RevisedDateOfPayment,
                            DateOfDeduction = pay.DateOfDeduction,
                            ReceiptNo = pay.ReceiptNo,
                            ClientPaymentID = pay.ClientPaymentID,
                            LotNo = pay.LotNo,
                            GrossShareAmount = cpt.GrossShareAmount,
                            AmountShare = cpt.ShareAmount,
                            RemittanceStatus = remSt.RemittanceStatusText,
                            ChallanAckNo = r.ChallanAckNo,
                            ChallanDate = r.ChallanDate,
                            ChallanAmount = cpt.TdsInterest + cpt.Tds + cpt.LateFee,
                            F16BDateOfReq = r.F16BDateOfReq,
                            F16BRequestNo = r.F16BRequestNo,
                            RemittanceStatusID = cpt.RemittanceStatusID,
                            CustomerPAN = cp.CustomerPAN,
                            OnlyTDS=cp.OnlyTDS??false,
                            TracesPassword = cp.TracesPassword

                        }).PreFilterRemittanceBy(request.Filter)
                                    .ToList()
                                    .AsQueryable()
                                    .PostFilterRemittanceBy(request.Filter)
                                    .ToList();
                return remittances;
            }

        }
    }
}
