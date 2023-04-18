using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ReProServices.Application.Common.Interfaces;

namespace ReProServices.Application.NatureOfPayments.Queries.GetNatureOfPayments
{
    public class GetNatureOfPaymentsQuery : IRequest<IList<NatureOfPaymentDto>>
    {
         public class GetNatureOfPaymentsQueryHandler : IRequestHandler<GetNatureOfPaymentsQuery, IList<NatureOfPaymentDto>>
    {

        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public GetNatureOfPaymentsQueryHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IList<NatureOfPaymentDto>> Handle(GetNatureOfPaymentsQuery request, CancellationToken cancellationToken)
        {
            var vm = await _context.NatureOfPayment
                .ProjectTo<NatureOfPaymentDto>(_mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken);
            return vm;

        }

    }
}
}
