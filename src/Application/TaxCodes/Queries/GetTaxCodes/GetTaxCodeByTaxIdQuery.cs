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
    public class GetTaxCodeByTaxIdQuery : IRequest<TaxCodesDto>
    {
        public int TaxId { get; set; }
        public class GetTaxCodeByTaxIdQueryHandler : IRequestHandler<GetTaxCodeByTaxIdQuery, TaxCodesDto>
        {
            private readonly IApplicationDbContext _context;
            private readonly IMapper _mapper;
            public GetTaxCodeByTaxIdQueryHandler(IApplicationDbContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            public async Task<TaxCodesDto> Handle(GetTaxCodeByTaxIdQuery request, CancellationToken cancellationToken)
            {
                var vm = await _context.TaxCodes
                    .Where(x => x.TaxID == request.TaxId)
                    .ProjectTo<TaxCodesDto>(_mapper.ConfigurationProvider)
                    .FirstOrDefaultAsync(cancellationToken);
                return vm;

            }
        }
    }
}
