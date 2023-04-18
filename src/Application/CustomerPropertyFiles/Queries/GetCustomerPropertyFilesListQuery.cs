using MediatR;
using System;
using System.Collections.Generic;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using ReProServices.Application.Common.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;

namespace ReProServices.Application.CustomerPropertyFiles.Queries
{
    public class GetCustomerPropertyFilesListQuery : IRequest<IList<CustomerPropertyFileDto>>
    {
        public Guid OwnershipID { get; set; }
        public bool GetFilesToo { get; set; }
        public int start { get; set; }
        public int end { get; set; }
        public class GetCustomerPropertyFilesListQueryHandler :
                              IRequestHandler<GetCustomerPropertyFilesListQuery, IList<CustomerPropertyFileDto>>
        {

            private readonly IApplicationDbContext _context;
            private readonly IMapper _mapper;

            public GetCustomerPropertyFilesListQueryHandler(IApplicationDbContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            public async Task<IList<CustomerPropertyFileDto>> Handle(GetCustomerPropertyFilesListQuery request, CancellationToken cancellationToken)
            {

                IList<CustomerPropertyFileDto> vm;
                if (request.GetFilesToo)
                {
                    vm = await _context.CustomerPropertyFile
                        .Where(x => x.OwnershipID == request.OwnershipID)
                        .ProjectTo<CustomerPropertyFileDto>(_mapper.ConfigurationProvider)
                        .ToListAsync(cancellationToken);
                }
                else
                {
                    vm = await _context.CustomerPropertyFile
                      .FromSqlRaw("Select  BlobID, OwnershipID, FileName, PanID, fileCategoryId, UploadTime = null,  " +
                      "           FileType = null, FileLength = null, FileBlob = null,GDfileID=null" +
                      "           FROM CustomerPropertyFile" +
                      "           WHERE  OwnershipID = {0} ", request.OwnershipID)
                      .ProjectTo<CustomerPropertyFileDto>(_mapper.ConfigurationProvider)
                       .ToListAsync(cancellationToken);
                }


                return vm;
            }
            
        }
    }
}
