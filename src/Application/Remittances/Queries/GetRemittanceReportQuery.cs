using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ReProServices.Application.Common.Interfaces;
using ReProServices.Application.TdsRemittance;

namespace ReProServices.Application.Remittances.Queries
{
    public class GetRemittanceReportQuery : IRequest<IList<RemittanceReportDto>>
    {
        public RemittanceReportFilter Filter { get; set; } = new RemittanceReportFilter();
        public class GetRemittanceReportQueryHandler : IRequestHandler<GetRemittanceReportQuery, IList<RemittanceReportDto>>
        {
            private readonly IApplicationDbContext _context;
            public GetRemittanceReportQueryHandler(IApplicationDbContext context)
            {
                _context = context;
            }

            public async Task<IList<RemittanceReportDto>> Handle(GetRemittanceReportQuery request, CancellationToken cancellationToken)
            {            
                var det = (from pay in _context.ClientPayment
                                 join cpt in _context.ClientPaymentTransaction on new { pay.InstallmentID, pay.ClientPaymentID } equals
                                     new { cpt.InstallmentID, cpt.ClientPaymentID }
                                 join rm in _context.Remittance on cpt.ClientPaymentTransactionID equals rm.ClientPaymentTransactionID
                                 join cp in _context.ViewCustomerPropertyExpanded on new { cpt.OwnershipID, cpt.CustomerID } equals new
                                 { cp.OwnershipID, cp.CustomerID }
                                 join sl in _context.ViewSellerPropertyBasic on cp.PropertyID equals sl.PropertyID
                                 where (request.Filter.SellerID >0)? sl.SellerID == request.Filter.SellerID : true
                                 where (request.Filter.PropertyID > 0) ? sl.PropertyID == request.Filter.PropertyID : true
                                 select new RemittanceReportDto
                                 {
                                     LotNo=pay.LotNo,
                                 ChallanID=rm.ChallanID,
                                     CustomerName = cp.CustomerName,
                                     Premises = cp.PropertyPremises,
                                     UnitNo = cp.UnitNo,
                                     ChallanDate=rm.ChallanDate,
                                     ChallanAckNo=rm.ChallanAckNo,
                                     ChallanAmount=rm.ChallanAmount,
                                     F16BDateOfReq=rm.F16BDateOfReq,
                                     F16BRequestNo=rm.F16BRequestNo,
                                     F16BCertificateNo=rm.F16BCertificateNo,
                                     F16UpdateDate=rm.F16UpdateDate,
                                     F16CreditedAmount=rm.F16CreditedAmount,
                                     DateOfPayment=pay.DateOfPayment,
                                     AmountPaid=pay.AmountPaid,
                                     GrossAmount=cpt.GrossShareAmount,
                                     GST=cpt.Gst,
                                     TdsRate=pay.TdsRate
                                 }).PreFilterReportBy(request.Filter)
                        .ToList()
                        .AsQueryable()
                        .PostFilterReportBy(request.Filter)
                        .ToList();

                return det;
            }
        }
    }
}
