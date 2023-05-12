using MediatR;
using ReProServices.Application.Common.Interfaces;
using System.Threading.Tasks;
using System.Threading;
using ReProServices.Domain.Entities;
using System.Linq;
using System;
using ReProServices.Application.CustomerPropertyFiles;

namespace ReProServices.Application.DebitAdvices.Commands
{
    public class DebitAdviceUploadFileCommand :IRequest<int>
    {
        public CustomerPropertyFileDto CustomerPropertyFile { get; set; }
        public class DebitAdviceUploadFileCommandHandler : IRequestHandler<DebitAdviceUploadFileCommand, int>
        {
            private readonly IApplicationDbContext _context;
            private readonly ICurrentUserService _currentUserService;
            public DebitAdviceUploadFileCommandHandler(IApplicationDbContext context, ICurrentUserService currentUserService)
            {
                _context = context;
                _currentUserService = currentUserService;
            }

            public async Task<int> Handle(DebitAdviceUploadFileCommand request, CancellationToken cancellationToken)
            {
                var customerPropertyFile = request.CustomerPropertyFile;


                CustomerPropertyFile fileEntity = new CustomerPropertyFile
                {
                    FileName = customerPropertyFile.FileName,
                    FileBlob = customerPropertyFile.FileBlob,
                    UploadTime = DateTime.Now,
                    FileLength = customerPropertyFile.FileBlob.Length,
                    FileType = customerPropertyFile.FileType,
                    FileCategoryId = customerPropertyFile.FileCategoryId,
                    GDfileID = customerPropertyFile.GDfileID
                };
                await _context.CustomerPropertyFile.AddAsync(fileEntity, cancellationToken);
                await _context.SaveChangesAsync(cancellationToken);


                var blobID = fileEntity.BlobID;


                return blobID;
            }
        }
    }
}
