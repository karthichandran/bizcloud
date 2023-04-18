using MediatR;
using ReProServices.Application.Common.Interfaces;
using ReProServices.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ReProServices.Application.Receipts.Commands.CreateReceipt
{
    public class CreateReceiptCommand : IRequest<int>
    {
        public IList<ReceiptDto> receiptList{ get; set; }

        public class CreateReceiptCommandhandler : IRequestHandler<CreateReceiptCommand, int>
        {
            private readonly IApplicationDbContext _context;
            private readonly ICurrentUserService _currentUserService;
            public CreateReceiptCommandhandler(IApplicationDbContext context, ICurrentUserService currentUserService)
            {
                _context = context;
                _currentUserService = currentUserService;
            }

            public async Task<int> Handle(CreateReceiptCommand request, CancellationToken cancellationToken)
            {
                var userInfo = _context.Users.FirstOrDefault(x => x.UserName == _currentUserService.UserName && x.IsActive);

                foreach (var dto in request.receiptList) {                   
                        Receipt entity = new Receipt
                        {
                            ReceiptType = dto.ReceiptType,
                            ClientPaymentTransactionID = dto.ClientPaymentTransactionID,
                            CustomerBillingID = dto.CustomerBillingID,
                            LotNo = dto.LotNo,
                            ServiceFee = dto.ServiceFee,
                            GstPayable = dto.GstPayable,
                            Tds = dto.Tds,
                            TdsInterest = dto.TdsInterest,
                            LateFee = dto.LateFee,
                            DateOfReceipt = dto.DateOfReceipt,
                            ModeOfReceiptID = dto.ModeOfReceiptID,
                            ReferenceNo = dto.ReferenceNo,
                            TdsReceived = dto.TdsReceived,
                            TotalServiceFeeReceived = dto.TotalServiceFeeReceived,
                            TdsInterestReceived = dto.TdsInterestReceived,
                            LateFeeReceived = dto.LateFeeReceived,
                            OwnershipID = dto.OwnershipID,
                            CustomerID = dto.CustomerID,
                            Created = DateTime.Now,
                            CreatedBy = userInfo.UserID.ToString()
                        };
                        await _context.Receipt.AddAsync(entity, cancellationToken);                  
                }              
              
                await _context.SaveChangesAsync(cancellationToken);
                return 0;
            }
        }
    }
}
