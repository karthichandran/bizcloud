using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using ReProServices.Application.Common.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ReProServices.Application.Remarks.Queries
{
    public class GetRemarksQuery : IRequest<IList<RemarkDto>>
    {
        public class GetRemarksQueryHandler : IRequestHandler<GetRemarksQuery, IList<RemarkDto>>
        {
            private readonly IApplicationDbContext _context;
            private readonly IMapper _mapper;

            public GetRemarksQueryHandler(IApplicationDbContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            public async Task<IList<RemarkDto>> Handle(GetRemarksQuery request, CancellationToken cancellationToken)
            {
                var vm = await _context.Remark
                    .ProjectTo<RemarkDto>(_mapper.ConfigurationProvider)
                    .ToListAsync(cancellationToken);
                return vm;
            }

        }
    }
}
