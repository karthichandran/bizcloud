using MediatR;
using ReProServices.Application.Common.Interfaces;
using System.Threading.Tasks;
using System.Threading;
using ReProServices.Domain.Entities;
using System.Linq;
using System;

namespace ReProServices.Application.BankAccount.Commands
{
    public class DeleteBankAccountCommand : IRequest<bool>
    {
        public int AccountId { get; set; }
        public class DeleteBankAccountCommandHandler : IRequestHandler<DeleteBankAccountCommand, bool>
        {
            private readonly IApplicationDbContext _context;
            private readonly ICurrentUserService _currentUserService;
            public DeleteBankAccountCommandHandler(IApplicationDbContext context, ICurrentUserService currentUserService)
            {
                _context = context;
                _currentUserService = currentUserService;
            }

            public async Task<bool> Handle(DeleteBankAccountCommand request, CancellationToken cancellationToken)
            {
                var entity = _context.BankAccountDetails.First(x => x.AccountId == request.AccountId);
                _ = _context.BankAccountDetails.Remove(entity);
                await _context.SaveChangesAsync(cancellationToken);
                return true;
            }
        }
    }
}
