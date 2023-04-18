using MediatR;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using ReProServices.Application.Common.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;

namespace ReProServices.Application.CustomerPropertyFiles.Queries
{
    public class GetCustomerPropertyFileByBlobIdQuery : IRequest<CustomerPropertyFileDto>
    {
        public int FileID { get; set; }
        public class GetCustomerPropertyFileByBlobIdQueryHandler :
                              IRequestHandler<GetCustomerPropertyFileByBlobIdQuery, CustomerPropertyFileDto>
        {

            private readonly IApplicationDbContext _context;
            private readonly IMapper _mapper;

            public GetCustomerPropertyFileByBlobIdQueryHandler(IApplicationDbContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            public async Task<CustomerPropertyFileDto> Handle(GetCustomerPropertyFileByBlobIdQuery request, CancellationToken cancellationToken)
            {
                var vm = await _context.CustomerPropertyFile
                        .Where(x => x.BlobID == request.FileID)
                        .ProjectTo<CustomerPropertyFileDto>(_mapper.ConfigurationProvider)
                        .FirstOrDefaultAsync(cancellationToken);

                return vm;
            }

        }
    }
}
