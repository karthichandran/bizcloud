using MediatR;
using ReProServices.Application.Common.Interfaces;
using ReProServices.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ReProServices.Application.Receipts.Commands.UpdateReceipt
{
    public class UpdateReceiptCommand : IRequest<int>
    {
        public IList<ReceiptDto> receiptList { get; set; }

        public class UpdateReceiptCommandhandler : IRequestHandler<UpdateReceiptCommand, int>
        {
            private readonly IApplicationDbContext _context;
            private readonly ICurrentUserService _currentUserService;
            public UpdateReceiptCommandhandler(IApplicationDbContext context, ICurrentUserService currentUserService)
            {
                _context = context;
                _currentUserService = currentUserService;
            }

            public async Task<int> Handle(UpdateReceiptCommand request, CancellationToken cancellationToken)
            {
                var userInfo = _context.Users.FirstOrDefault(x => x.UserName == _currentUserService.UserName && x.IsActive);
                foreach (var dto in request.receiptList)
                {
                    var entity = _context.Receipt
                   .First(x => x.ReceiptID == dto.ReceiptID);
                    entity.DateOfReceipt = dto.DateOfReceipt;
                    entity.ModeOfReceiptID = dto.ModeOfReceiptID;
                    entity.ReferenceNo = dto.ReferenceNo;
                    entity.TdsReceived = dto.TdsReceived;
                    entity.TotalServiceFeeReceived = dto.TotalServiceFeeReceived;
                    entity.TdsInterestReceived = dto.TdsInterestReceived;
                    entity.LateFeeReceived = dto.LateFeeReceived;
                    entity.Updated = DateTime.Now;
                    entity.UpdatedBy = userInfo.UserID.ToString();
                    _context.Receipt.Update(entity);
                }                    
                
                await _context.SaveChangesAsync(cancellationToken);
                return 0;
            }
        }
    }
}
