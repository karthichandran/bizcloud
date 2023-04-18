using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using ReProServices.Application.Common.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ReProServices.Application.Sellers.Queries.GetSellers
{
    public class GetSellersQuery : IRequest<IList<SellerDto>>
    {
        public SellerFilter Filter { get; set; } = new SellerFilter();

        public class GetSellersQueryHandler : IRequestHandler<GetSellersQuery, IList<SellerDto>>
        {
            private readonly IApplicationDbContext _context;
            private readonly IMapper _mapper;

            public GetSellersQueryHandler(IApplicationDbContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            public async Task<IList<SellerDto>> Handle(GetSellersQuery request, CancellationToken cancellationToken)
            {
                var vm = await _context.Seller
                    .FilterSellersBy(request.Filter)
                    .ProjectTo<SellerDto>(_mapper.ConfigurationProvider)
                    .ToListAsync(cancellationToken);
                return vm;
            }

        }
    }
}
