using MediatR;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using ReProServices.Application.Common.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ReProServices.Application.TaxCodes.Queries.GetTaxCodes
{
    public class GetTaxCodesByIdQuery : IRequest<TaxCodesDto>
    {
        public int TaxCodeId { get; set; }
        public class GetTaxCodesByIdQueryHandler : IRequestHandler<GetTaxCodesByIdQuery, TaxCodesDto>
        {
            private readonly IApplicationDbContext _context;
            private readonly IMapper _mapper;

            public GetTaxCodesByIdQueryHandler(IApplicationDbContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            public async Task<TaxCodesDto> Handle(GetTaxCodesByIdQuery request, CancellationToken cancellationToken)
            {
                //picking Active Tax Code
                var vm = await _context.TaxCodes
                    .Where(x => x.TaxCodeId == request.TaxCodeId
                           && x.EffectiveEndDate >= System.DateTime.Now
                           && x.EffectiveStartDate <= System.DateTime.Now)
                    .ProjectTo<TaxCodesDto>(_mapper.ConfigurationProvider)
                    .FirstOrDefaultAsync(cancellationToken);
                return vm;
            }

        }
    }
}
