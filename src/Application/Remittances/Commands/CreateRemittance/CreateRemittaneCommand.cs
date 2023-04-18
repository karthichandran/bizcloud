using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using ReProServices.Application.ClientPayments.Commands;
using ReProServices.Application.Common.Interfaces;
using ReProServices.Domain.Entities;

namespace ReProServices.Application.Remittances.Commands.CreateRemittance
{
    public class CreateRemittaneCommand : IRequest<int>
    {
        public RemittanceDto RemittanceDto { get; set; }

        public class CreateRemittaneCommandHandler : IRequestHandler<CreateRemittaneCommand, int>
        {
            private readonly IApplicationDbContext _context;
            private readonly ICurrentUserService _currentUserService;
            public CreateRemittaneCommandHandler(IApplicationDbContext context, ICurrentUserService currentUserService)
            {
                _context = context;
                _currentUserService = currentUserService;
            }

            public async Task<int> Handle(CreateRemittaneCommand request, CancellationToken cancellationToken)
            {
                var userInfo = _context.Users.FirstOrDefault(x => x.UserName == _currentUserService.UserName && x.IsActive);
                var dto = request.RemittanceDto;
                Remittance entity = new Remittance
                {
                    ClientPaymentTransactionID = dto.ClientPaymentTransactionID,
                    ChallanAckNo = dto.ChallanAckNo.Trim(),
                    ChallanAmount = dto.ChallanAmount,
                    ChallanDate = dto.ChallanDate,
                    ChallanID = dto.ChallanID.Trim(),
                    Created = DateTime.Now,
                    CreatedBy = userInfo == null ? "desktop" : userInfo.UserID.ToString(),
                    ChallanIncomeTaxAmount=dto.ChallanIncomeTaxAmount,
                    ChallanInterestAmount=dto.ChallanInterestAmount,
                    ChallanFeeAmount=dto.ChallanFeeAmount,
                    ChallanCustomerName=dto.ChallanCustomerName
                };
                await  _context.Remittance.AddAsync(entity, cancellationToken);
                await _context.SaveChangesAsync(cancellationToken);

                return entity.RemittanceID;
            }
        }
    }
}
