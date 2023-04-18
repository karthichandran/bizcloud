using MediatR;
using ReProServices.Application.Common.Interfaces;
using System.Threading.Tasks;
using System.Threading;
using ReProServices.Domain.Entities;
using System.Collections.Generic;
using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using ReProServices.Domain.Enums;

namespace ReProServices.Application.CustomerPropertyFiles.Commands.UploadCustomerProeprtyFile
{
    public class UploadRemittanceFileCommand : IRequest<Unit>
    {
        public CustomerPropertyFileDto CustomerPropertyFile { get; set; }
        public int RemittanceID { get; set; }
        public class UploadCustomerPropertyFileCommandHandler : IRequestHandler<UploadRemittanceFileCommand, Unit>
        {
            private readonly IApplicationDbContext _context;
            public UploadCustomerPropertyFileCommandHandler(IApplicationDbContext context)
            {
                _context = context;
            }

            public async Task<Unit> Handle(UploadRemittanceFileCommand request, CancellationToken cancellationToken)
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
                    GDfileID=customerPropertyFile.GDfileID
                };
                await _context.CustomerPropertyFile.AddAsync(fileEntity, cancellationToken);
                await _context.SaveChangesAsync(cancellationToken);


                var blobID = fileEntity.BlobID;

                var rem = _context.Remittance
                    .First(x => x.RemittanceID == request.RemittanceID);
                if (fileEntity.FileCategoryId == (int)EFileCategory.Form16)
                    rem.Form16BlobID = blobID;
                if (fileEntity.FileCategoryId == (int)EFileCategory.Challan)
                    rem.ChallanBlobID = blobID;

                _context.Remittance.Update(rem).State = EntityState.Modified;
                await _context.SaveChangesAsync(cancellationToken);

                return Unit.Value;
            }
        }
    }
}
