using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ReProServices.Application.Common.Interfaces;

namespace ReProServices.Application.RemittanceStatusList.Queries
{
    public class GetRemittanceStatusQuery : IRequest<IList<RemittanceStatusDto>>
    {
        public class GetRemittanceStatusQueryHandler : IRequestHandler<GetRemittanceStatusQuery, IList<RemittanceStatusDto>>
        {
            private readonly IApplicationDbContext _context;
            private readonly IMapper _mapper;

            public GetRemittanceStatusQueryHandler(IApplicationDbContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            public async Task<IList<RemittanceStatusDto>> Handle(GetRemittanceStatusQuery request, CancellationToken cancellationToken)
            {
                var vm = await _context.RemittanceStatus
                    .ProjectTo<RemittanceStatusDto>(_mapper.ConfigurationProvider)
                    .ToListAsync(cancellationToken);
                return vm;

            }

        }
    }
}
