using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ReProServices.Application.Common.Interfaces;

namespace ReProServices.Application.StatusTypes.Queries.GetStatusTypes
{
    public class GetStatusTypeQuery : IRequest<IList<StatusTypeDto>>
    {
         public class GetStatusTypeQueryHandler : IRequestHandler<GetStatusTypeQuery, IList<StatusTypeDto>>
    {

        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public GetStatusTypeQueryHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IList<StatusTypeDto>> Handle(GetStatusTypeQuery request, CancellationToken cancellationToken)
        {
            var vm = await _context.StatusType
                .ProjectTo<StatusTypeDto>(_mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken);
            return vm;

        }

    }
}
}
