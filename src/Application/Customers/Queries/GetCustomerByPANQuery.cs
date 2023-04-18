using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ReProServices.Application.Common.Interfaces;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ReProServices.Application.Customers.Queries
{
    public class GetCustomerByPANQuery : IRequest<CustomerDto>
    {
        public string PAN { get; set; }
        public class GetCustomerByPANQueryHandler : IRequestHandler<GetCustomerByPANQuery, CustomerDto>
        {
            private readonly IApplicationDbContext _context;
            private readonly IMapper _mapper;
            public GetCustomerByPANQueryHandler(IApplicationDbContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            public async Task<CustomerDto> Handle(GetCustomerByPANQuery request, CancellationToken cancellationToken)
            {
                var custDto = await _context.Customer
                    .Where(c =>  c.PAN == request.PAN.TrimEnd())
                    .ProjectTo<CustomerDto>(_mapper.ConfigurationProvider)
                    .FirstOrDefaultAsync(cancellationToken);
                return custDto;
            }
        }
    }
}
