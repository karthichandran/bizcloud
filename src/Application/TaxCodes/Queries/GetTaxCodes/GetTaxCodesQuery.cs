using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using ReProServices.Application.Common.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace ReProServices.Application.TaxCodes.Queries.GetTaxCodes
{
    public class GetTaxCodesQuery : IRequest<List<TaxCodesDto>>
    {
        public TaxCodesFilter Filter { get; set; } = new TaxCodesFilter();

        public class GetTaxCodesQueryHandler : IRequestHandler<GetTaxCodesQuery, List<TaxCodesDto>>
        {
            private readonly IApplicationDbContext _context;
            private readonly IMapper _mapper;

            public GetTaxCodesQueryHandler(IApplicationDbContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            public async Task<List<TaxCodesDto>> Handle(GetTaxCodesQuery request, CancellationToken cancellationToken)
            {
                var vm = await _context.TaxCodes
                    //.Where(x => x.EffectiveEndDate >= System.DateTime.Now
                    //       && x.EffectiveStartDate <= System.DateTime.Now)
                    //.FilterTaxCodesBy(request.Filter) //todo revisit this commented piece and handle data import
                    .ProjectTo<TaxCodesDto>(_mapper.ConfigurationProvider)
                    .ToListAsync(cancellationToken);
                return vm;
            }

        }
    }
}
