using MediatR;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using ReProServices.Application.Common.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ReProServices.Application.Sellers.Queries.GetSellers
{
    public class GetSellersByIdQuery : IRequest<SellerDto>
    {
        public int SellerID { get; set; }
        public class GetSellerByIdQueryQueryHandler : IRequestHandler<GetSellersByIdQuery, SellerDto>
        {
            private readonly IApplicationDbContext _context;
            private readonly IMapper _mapper;

            public GetSellerByIdQueryQueryHandler(IApplicationDbContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            public async Task<SellerDto> Handle(GetSellersByIdQuery request, CancellationToken cancellationToken)
            {
                var vm = await _context.Seller
                    .Where(x => x.SellerID == request.SellerID)
                    .ProjectTo<SellerDto>(_mapper.ConfigurationProvider)
                    .FirstOrDefaultAsync(cancellationToken);
                return vm;
            }

        }
    }
}
