using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using ReProServices.Application.Common.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq;
namespace ReProServices.Application.BankAccount.Queries
{
    public class GetBankAccountByIdQuery : IRequest<BankAccountDetailsDto>
    {
        public int accountId { get; set; }
        public class GetBankAccountByIdQueryHandler : IRequestHandler<GetBankAccountByIdQuery, BankAccountDetailsDto>
        {

            private readonly IApplicationDbContext _context;
            private readonly IMapper _mapper;

            public GetBankAccountByIdQueryHandler(IApplicationDbContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            public async Task<BankAccountDetailsDto> Handle(GetBankAccountByIdQuery request, CancellationToken cancellationToken)
            {
                var vm = await _context.BankAccountDetails.Where(x => x.AccountId == request.accountId)
                    .ProjectTo<BankAccountDetailsDto>(_mapper.ConfigurationProvider)
                    .FirstOrDefaultAsync(cancellationToken);                  

                return vm;

            }

        }
    }
}
