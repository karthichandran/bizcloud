using MediatR;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using ReProServices.Application.Common.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using ReProServices.Application.SellerProperties;

namespace ReProServices.Application.Property.Queries
{
    public class GetPropertyByIdQuery : IRequest<PropertyVM>
    {
        public int PropertyID { get; set; }
        public class GetPropertiesByIdQueryHandler : IRequestHandler<GetPropertyByIdQuery, PropertyVM>
        {

            private readonly IApplicationDbContext _context;
            private readonly IMapper _mapper;

            public GetPropertiesByIdQueryHandler(IApplicationDbContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            public async Task<PropertyVM> Handle(GetPropertyByIdQuery request, CancellationToken cancellationToken)
            {

                
                var prop = await _context.Property
                    .Where(x => x.PropertyID == request.PropertyID)
                    .ProjectTo<PropertyDto>(_mapper.ConfigurationProvider)
                    .FirstOrDefaultAsync(cancellationToken);

                var sellerProperties = await _context.SellerProperty
                   .Where(x => x.PropertyID == request.PropertyID)
                   .ProjectTo<SellerPropertyDto>(_mapper.ConfigurationProvider)
                   .ToListAsync(cancellationToken);


                var vm = new PropertyVM() {
                    propertyDto = prop,
                    sellerProperties = sellerProperties
                };
                return vm;

            }

        }
    }
}
