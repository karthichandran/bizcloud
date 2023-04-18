using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using ReProServices.Application.Common.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ReProServices.Application.States.Queries.GetStates
{
    public class GetStatesQuery :IRequest<IList<StateDto>>
    {
         public class GetStatesQueryHandler : IRequestHandler<GetStatesQuery, IList<StateDto>>
    {

        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public GetStatesQueryHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IList<StateDto>> Handle(GetStatesQuery request, CancellationToken cancellationToken)
        {
            var vm = await _context.StateList
                .ProjectTo<StateDto>(_mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken);
            return vm;

        }

    }
}
}
