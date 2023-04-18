using System;
using MediatR;
using ReProServices.Application.Common.Interfaces;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using ReProServices.Domain.Extensions;
using ReProServices.Application.BankAccount;

namespace ReProServices.Application.AutoFill
{
   public class GetBankAccountDetailsQuery: IRequest<BankAccountDetailsDto>
    {
        public class GetBankAccountDetailsQueryHandler : IRequestHandler<GetBankAccountDetailsQuery, BankAccountDetailsDto> {
            private readonly IApplicationDbContext _context;
            public GetBankAccountDetailsQueryHandler(IApplicationDbContext context) {
                _context = context;
            }

            public async Task<BankAccountDetailsDto> Handle(GetBankAccountDetailsQuery request, CancellationToken cancellationToken) {

                var vm =await _context.BankAccountDetails.Select(x=> new BankAccountDetailsDto { 
                UserName=x.UserName,
                UserPassword=x.UserPassword
                }) .FirstOrDefaultAsync(cancellationToken);
                return vm;
            }
        }
    }
}
