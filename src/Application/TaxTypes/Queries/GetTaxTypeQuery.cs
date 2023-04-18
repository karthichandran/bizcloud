using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using ReProServices.Application.Common.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using MediatR;

namespace ReProServices.Application.TaxTypes.Queries
{
    public class GetTaxTypeQuery : IRequest<IList<TaxTypeDto>>
    {
        public class GetTaxTypeQueryHandler : IRequestHandler<GetTaxTypeQuery, IList<TaxTypeDto>>
        {
            private readonly IApplicationDbContext _context;
            private readonly IMapper _mapper;

            public GetTaxTypeQueryHandler(IApplicationDbContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            public async Task<IList<TaxTypeDto>> Handle(GetTaxTypeQuery request, CancellationToken cancellationToken)
            {
                var tt = await _context.TaxType
                    .ProjectTo<TaxTypeDto>(_mapper.ConfigurationProvider)
                    .ToListAsync(cancellationToken);
                return tt;
            }

        }
    }
}
