using MediatR;
using ReProServices.Application.Common.Interfaces;
using System.Threading.Tasks;
using System.Threading;
using ReProServices.Domain.Entities;
using System.Linq;
using System;

namespace ReProServices.Application.BankAccount.Commands
{
    public class CreateBankAccountCommand: IRequest<int>
    {
        public BankAccountDetailsDto bankDetails { get; set; }
        public class CreateBankAccountCommandHandler : IRequestHandler<CreateBankAccountCommand, int>
        {
            private readonly IApplicationDbContext _context;
            private readonly ICurrentUserService _currentUserService;
            public CreateBankAccountCommandHandler(IApplicationDbContext context, ICurrentUserService currentUserService)
            {
                _context = context;
                _currentUserService = currentUserService;
            }

            public async Task<int> Handle(CreateBankAccountCommand request, CancellationToken cancellationToken)
            {
                var obj = request.bankDetails;
                var entity = new BankAccountDetails
                {
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

                await _context.BankAccountDetails.AddAsync(entity, cancellationToken);
                await _context.SaveChangesAsync(cancellationToken);
                return entity.AccountId;
            }
        }
    }
}
