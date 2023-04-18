using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using ReProServices.Application.Common.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using MediatR;

namespace ReProServices.Application.ModeOfReceipt.Queries
{
    public class GetModeOfReceiptQuery : IRequest<IList<ModeOfReceiptDto>>
    {
        public class GetMethodOfReceiptQueryHandler : IRequestHandler<GetModeOfReceiptQuery, IList<ModeOfReceiptDto>>
        {
            private readonly IApplicationDbContext _context;
            private readonly IMapper _mapper;

            public GetMethodOfReceiptQueryHandler(IApplicationDbContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            public async Task<IList<ModeOfReceiptDto>> Handle(GetModeOfReceiptQuery request, CancellationToken cancellationToken)
            {
                var tt = await _context.ModeOfReceipt
                    .ProjectTo<ModeOfReceiptDto>(_mapper.ConfigurationProvider)
                    .ToListAsync(cancellationToken);
                return tt;
            }

        }
    }
}
