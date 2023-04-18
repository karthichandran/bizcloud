using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using ReProServices.Application.Common.Interfaces;
using Microsoft.EntityFrameworkCore;
namespace ReProServices.Application.Receipts.Queries
{
     public class GetReceiptByIdQuery : IRequest<ReceiptDto>
    {
        public int ReceiptId { get; set; } = 0;
        public class GetReceiptByIdQueryHandler : IRequestHandler<GetReceiptByIdQuery, ReceiptDto>
        {
            private readonly IApplicationDbContext _context;
            public GetReceiptByIdQueryHandler(IApplicationDbContext context)
            {
                _context = context;
            }

            public async Task<ReceiptDto> Handle(GetReceiptByIdQuery request, CancellationToken cancellationToken)
            {
                var rawList = new List<ReceiptDto>();

                //var vm = (from pay in _context.ClientPayment
                //          join cpt in _context.ClientPaymentTransaction on new { pay.InstallmentID, pay.ClientPaymentID } equals new { cpt.InstallmentID, cpt.ClientPaymentID }
                //          join cp in _context.ViewCustomerPropertyExpanded on new { cpt.OwnershipID, cpt.CustomerID } equals new { cp.OwnershipID, cp.CustomerID }
                //          join cb in _context.CustomerBilling on new { cp.OwnershipID, cp.CustomerID } equals new { cb.OwnershipID, cb.CustomerID }
                //          join rp in _context.Receipt on cpt.ClientPaymentTransactionID equals rp.ClientPaymentTransactionID
                var vm = await (from rp in _context.ViewReceipt
                          join pt in _context.Property on rp.PropertyID equals pt.PropertyID
                          where rp.ReceiptID == request.ReceiptId
                          select new ReceiptDto
                          {
                              ReceiptID = rp.ReceiptID,
                              OwnershipID = rp.OwnershipID,
                              Tds = rp.Tds,
                              LotNo = rp.LotNo,
                              CustomerName = rp.CustomerName,
                              Premises=pt.AddressPremises,
                              PropertyID = pt.PropertyID,                             
                              UnitNo = rp.UnitNo,
                              CustomerBillingID = rp.CustomerBillingID,
                              ServiceFee = rp.ServiceFee,
                              GstPayable = rp.GstPayable,
                              TotalServiceFeeReceived = rp.TotalServiceFeeReceived,
                              TdsInterest = rp.TdsInterest,
                              TdsInterestReceived=rp.TdsInterestReceived,
                              LateFee = rp.LateFee,
                              LateFeeReceived=rp.LateFeeReceived,
                              ReferenceNo=rp.ReferenceNo
                            
                          }).FirstOrDefaultAsync(cancellationToken);

                
                return vm;

            }
        }
    }
}
