using MediatR;
using ReProServices.Application.Common.Interfaces;
using System.Threading.Tasks;
using System.Threading;
using System;
using ReProServices.Domain.Entities;
using ReProServices.Application.Common.Formulas;
using System.Linq;
using System.Transactions;
using ReProServices.Domain.Enums;

namespace ReProServices.Application.ClientPayments.Commands
{
    public class UpdateRemittanceStatusCommand : IRequest<Unit>
    {
        public int ClientPaymentTransactionID { get; set; }
        public int StatusID { get; set; }
        public class UpdateRemittanceStatusCommandHandler : IRequestHandler<UpdateRemittanceStatusCommand, Unit>
        {
            private readonly IApplicationDbContext _context;
            private readonly ICurrentUserService _currentUserService;
            public UpdateRemittanceStatusCommandHandler(IApplicationDbContext context, ICurrentUserService currentUserService)
            {
                _context = context;
                _currentUserService = currentUserService;
            }

            public async Task<Unit> Handle(UpdateRemittanceStatusCommand request, CancellationToken cancellationToken)
            {
                var userInfo = _context.Users.FirstOrDefault(x => x.UserName == _currentUserService.UserName && x.IsActive);
                var entity = _context.ClientPaymentTransaction
                                                .First(x => x.ClientPaymentTransactionID == request.ClientPaymentTransactionID);
                entity.RemittanceStatusID = request.StatusID;
                entity.Updated = DateTime.Now;
                entity.UpdatedBy = userInfo == null ? "desktop" : userInfo.UserID.ToString();

                _ = _context.ClientPaymentTransaction.Update(entity);
                await _context.SaveChangesAsync(cancellationToken);
                return Unit.Value;
            }

        }
    }
}
