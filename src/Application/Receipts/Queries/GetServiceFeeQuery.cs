using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using ReProServices.Application.Common.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ReProServices.Application.Receipts.Queries
{
    public class GetServiceFeeQuery : IRequest<IList<ReceiptDto>>
    {
        public ReceiptFilter Filter { get; set; } = new ReceiptFilter();

        public class GetServiceFeeQueryHandler : IRequestHandler<GetServiceFeeQuery, IList<ReceiptDto>> {
            private readonly IApplicationDbContext _context;
            public GetServiceFeeQueryHandler(IApplicationDbContext context)
            {
                _context = context;
            }

            public async Task<IList<ReceiptDto>> Handle(GetServiceFeeQuery request, CancellationToken cancellationToken)
            {
                var rawList = new List<ReceiptDto>();

                var vm = (from cpt in _context.ClientPaymentTransaction 
                          join cp in _context.ViewCustomerPropertyExpanded on new { cpt.OwnershipID, cpt.CustomerID } equals new { cp.OwnershipID, cp.CustomerID }
                          join cb in _context.CustomerBilling on new { cp.OwnershipID, cp.CustomerID } equals new { cb.OwnershipID, cb.CustomerID }
                          join rt  in _context.Receipt on cb.CustomerBillID equals rt.CustomerBillingID
                          where request.Filter.PropertyID > 0 ? cp.PropertyID == request.Filter.PropertyID : true
                          && request.Filter.SellerID > 0 ? cpt.SellerID == request.Filter.SellerID : true
                          select new ReceiptDto
                          {     
                              ReceiptID=rt.ReceiptID,
                              OwnershipID = rt.OwnershipID,                             
                              CustomerName = cp.CustomerName,
                              PropertyID = cp.PropertyID,
                              SellerID = cpt.SellerID,
                              UnitNo = cp.UnitNo,
                              CustomerBillingID = rt.CustomerBillingID,
                              ServiceFee =rt.ServiceFee,
                              GstPayable = rt.GstPayable,
                              TotalServiceFeeReceived =rt.TotalServiceFeeReceived,
                              TdsInterest = rt.TdsInterest,
                              LateFee = rt.LateFee,
                              LateFeeReceived=rt.LateFeeReceived,
                              ReferenceNo=rt.ReferenceNo,
                              ModeOfReceiptID=rt.ModeOfReceiptID,
                              DateOfReceipt=rt.DateOfReceipt,
                              EmailSent=rt.EmailSent,
                              EmailSentDate=rt.EmailSentDate,
                              CustomerID = cb.CustomerID
                          }).Distinct()
                               .PreFilterReceiptsBy(request.Filter)
                               .ToList()
                               .AsQueryable()
                               .PostFilterReceiptsBy(request.Filter)
                               .ToList();

                return vm;

            }
        }
    }
}
