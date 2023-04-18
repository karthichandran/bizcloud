using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using ReProServices.Application.Common.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ReProServices.Application.Property.Queries
{
    public class GetPropertiesQuery : IRequest<IList<PropertyDto>>
    {
        public PropertyFilter Filter { get; set; } = new PropertyFilter();
        public class GetPropertiesQueryHandler : IRequestHandler<GetPropertiesQuery, IList<PropertyDto>>
        {

            private readonly IApplicationDbContext _context;
            private readonly IMapper _mapper;

            public GetPropertiesQueryHandler(IApplicationDbContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            public async Task<IList<PropertyDto>> Handle(GetPropertiesQuery request, CancellationToken cancellationToken)
            {
                var vm = await _context.Property
                    .FilterPropertiesBy(request.Filter)
                    .ProjectTo<PropertyDto>(_mapper.ConfigurationProvider)
                    .ToListAsync(cancellationToken);

            
                return vm;

            }

        }
    }
}
