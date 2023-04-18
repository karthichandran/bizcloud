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
    public class GetBankAccountListQuery : IRequest<IList<BankAccountDetailsDto>>
    {
        public BankAccountFilter Filter { get; set; }
        public class GetBankAccountListQueryHandler : IRequestHandler<GetBankAccountListQuery, IList<BankAccountDetailsDto>>
        {

            private readonly IApplicationDbContext _context;
            private readonly IMapper _mapper;

            public GetBankAccountListQueryHandler(IApplicationDbContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            public async Task<IList<BankAccountDetailsDto>> Handle(GetBankAccountListQuery request, CancellationToken cancellationToken)
            {
                var vm = await _context.BankAccountDetails.ProjectTo<BankAccountDetailsDto>(_mapper.ConfigurationProvider)
                    .ToListAsync(cancellationToken);

                if (!string.IsNullOrEmpty(request.Filter.UserName)) {
                    vm = vm.Where(x => x.UserName.Contains(request.Filter.UserName)).ToList();
                }

                return vm;

            }

        }
    }
}
