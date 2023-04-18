using MediatR;
using ReProServices.Application.Common.Interfaces;
using System.Threading.Tasks;
using System.Threading;
using ReProServices.Domain.Entities;
using System.Collections.Generic;
using System;

namespace ReProServices.Application.CustomerPropertyFiles.Commands.UploadCustomerProeprtyFile
{
    public class UploadPanFileCommand : IRequest<int> 
    {
        public CustomerPropertyFileDto CustomerPropertyFile { get; set; }

        public class UploadPanFileCommandHandler : IRequestHandler<UploadPanFileCommand, int>
        {
            private readonly IApplicationDbContext _context;
            public UploadPanFileCommandHandler(IApplicationDbContext context)
            {
                _context = context;
            }

            public async Task<int> Handle(UploadPanFileCommand request, CancellationToken cancellationToken)
            {
                var customerPropertyFile = request.CustomerPropertyFile;               

                    CustomerPropertyFile fileEntity = new CustomerPropertyFile
                    {
                        FileName = customerPropertyFile.FileName,
                        FileBlob = customerPropertyFile.FileBlob,
                        OwnershipID = customerPropertyFile.OwnershipID,
                        UploadTime = DateTime.Now,
                        FileLength = customerPropertyFile.FileBlob.Length,
                        FileType = customerPropertyFile.FileType,
                        PanID = customerPropertyFile.PanID,
                        FileCategoryId = customerPropertyFile.FileCategoryId,
                        GDfileID = customerPropertyFile.GDfileID
                    };

                    if (fileEntity.OwnershipID.ToString() == "00000000-0000-0000-0000-000000000000")
                    { throw new ApplicationException("system tried to insert invalid document info. Please contact support"); }
                    await _context.CustomerPropertyFile.AddAsync(fileEntity, cancellationToken);
                    await _context.SaveChangesAsync(cancellationToken);              

                return fileEntity.BlobID;
            }
        }
    }
}
