using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using ReProServices.Application.ClientPayments.Commands;
using ReProServices.Application.Common.Interfaces;
using ReProServices.Domain.Enums;

namespace ReProServices.Application.Remittances.Commands.UpdateRemittance
{
    public class UpdateRemittaneCommand : IRequest<int>
    {
        public RemittanceDto RemittanceDto { get; set; }

        public class UpdateRemittaneCommandHandler : IRequestHandler<UpdateRemittaneCommand, int>
        {
            private readonly IApplicationDbContext _context;
            private readonly ICurrentUserService _currentUserService;
            public UpdateRemittaneCommandHandler(IApplicationDbContext context, ICurrentUserService currentUserService)
            {
                _context = context;
                _currentUserService = currentUserService;
            }

            public async Task<int> Handle(UpdateRemittaneCommand request, CancellationToken cancellationToken)
            {
                var userInfo = _context.Users.FirstOrDefault(x => x.UserName == _currentUserService.UserName && x.IsActive);
                var dto = request.RemittanceDto;
                var entity = _context.Remittance
                    .First(x => x.RemittanceID == dto.RemittanceID);

                entity.ClientPaymentTransactionID = dto.ClientPaymentTransactionID;
                entity.ChallanAckNo = dto.ChallanAckNo;
                entity.ChallanAmount = dto.ChallanAmount;
                entity.ChallanDate = dto.ChallanDate;
                entity.ChallanID = dto.ChallanID;
                entity.F16BCertificateNo = dto.F16BCertificateNo;
                entity.F16BDateOfReq = dto.F16BDateOfReq;
                entity.F16BRequestNo = dto.F16BRequestNo;
                entity.F16CustName = dto.F16CustName;
                entity.F16UpdateDate = dto.F16UpdateDate;
                entity.F16CreditedAmount = dto.F16CreditedAmount;
                entity.Updated= DateTime.Now;
                entity.UpdatedBy = userInfo == null ? "desktop" : userInfo.UserID.ToString();
                _context.Remittance.Update(entity);
                await _context.SaveChangesAsync(cancellationToken);

                return entity.RemittanceID;
            }
        }
    }
}
