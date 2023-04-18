using MediatR;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using ReProServices.Application.Common.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace ReProServices.Application.CustomerPropertyFiles.Queries
{
    public class GetCustomerPropertyFileByPanIdQuery : IRequest<CustomerPropertyFileDto>
    {
        public string PanID { get; set; }
        public class GetCustomerPropertyFileByPanIdQueryHandler :
                              IRequestHandler<GetCustomerPropertyFileByPanIdQuery, CustomerPropertyFileDto>
        {

            private readonly IApplicationDbContext _context;
            private readonly IMapper _mapper;

            public GetCustomerPropertyFileByPanIdQueryHandler(IApplicationDbContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            public async Task<CustomerPropertyFileDto> Handle(GetCustomerPropertyFileByPanIdQuery request, CancellationToken cancellationToken)
            {
                var vm = await _context.CustomerPropertyFile
                  .FromSqlRaw("Select  BlobID, OwnershipID, FileName, PanID,fileCategoryId, UploadTime = null,  " +
                  "           FileType = null, FileLength = null, FileBlob = null,GDfileID=null" +
                  "           FROM CustomerPropertyFile" +
                  "           WHERE  PanID = {0} ", request.PanID)
                  .ProjectTo<CustomerPropertyFileDto>(_mapper.ConfigurationProvider)
                   .FirstOrDefaultAsync(cancellationToken);



                return vm;
            }

        }
    }
}
