using MediatR;
using ReProServices.Application.Common.Interfaces;
using System.Threading.Tasks;
using System.Threading;
using ReProServices.Domain.Entities;
using System.Linq;
using System;

namespace ReProServices.Application.BankAccount.Commands
{
    public class UpdateBankAccountCommand : IRequest<int>
    {
        public BankAccountDetailsDto bankDetails { get; set; }
        public class UpdateBankAccountCommandHandler : IRequestHandler<UpdateBankAccountCommand, int>
        {
            private readonly IApplicationDbContext _context;
            private readonly ICurrentUserService _currentUserService;
            public UpdateBankAccountCommandHandler(IApplicationDbContext context, ICurrentUserService currentUserService)
            {
                _context = context;
                _currentUserService = currentUserService;
            }

            public async Task<int> Handle(UpdateBankAccountCommand request, CancellationToken cancellationToken)
            {
                var obj = request.bankDetails;
                var entity = new BankAccountDetails
                {
                    AccountId=obj.AccountId,
                    UserName = obj.UserName,
                    UserPassword = obj.UserPassword,
                    BankName = obj.BankName,
                    LaneNo = obj.LaneNo,
                    LetterA = obj.LetterA,
                    LetterB = obj.LetterB,
                    LetterC = obj.LetterC,
                    LetterD = obj.LetterD,
                    LetterE = obj.LetterE,
                    LetterF = obj.LetterF,
                    LetterG = obj.LetterG,
                    LetterH = obj.LetterH,
                    LetterI = obj.LetterI,
                    LetterJ = obj.LetterJ,
                    LetterK = obj.LetterK,
                    LetterL = obj.LetterL,
                    LetterM = obj.LetterM,
                    LetterN = obj.LetterN,
                    LetterO = obj.LetterO,
                    LetterP = obj.LetterP
                };

                _context.BankAccountDetails.Update(entity);
                await _context.SaveChangesAsync(cancellationToken);
                return entity.AccountId;
            }
        }
    }
}
