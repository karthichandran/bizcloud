using MediatR;
using ReProServices.Application.Common.Interfaces;
using System.Threading.Tasks;
using System.Threading;
using ReProServices.Domain.Entities;
using System.Collections.Generic;
using System;
using System.Linq;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace ReProServices.Application.CustomerPropertyFiles.Commands.UploadCustomerProeprtyFile
{
    public class UploadCustomerPropertyFileCommand : IRequest<bool> //todo send an improved response to client
    {
        public IList<CustomerPropertyFileDto> CustomerPropertyFiles { get; set; }

        public class UploadCustomerPropertyFileCommandHandler : IRequestHandler<UploadCustomerPropertyFileCommand, bool>
        {
            private readonly IApplicationDbContext _context;
            public UploadCustomerPropertyFileCommandHandler(IApplicationDbContext context)
            {
                _context = context;
            }

            public async Task<bool> Handle(UploadCustomerPropertyFileCommand request, CancellationToken cancellationToken)
            {
                foreach (var customerPropertyFile in request.CustomerPropertyFiles)
                {
                   // CustomerPropertyFile entity = null;

                    //if (customerPropertyFile.BlobID > 0)
                    //    entity = _context.CustomerPropertyFile.FirstOrDefault(x => x.BlobID == customerPropertyFile.BlobID);

                    //if (entity == null)
                    //{

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
                    //}
                    //else
                    //{
                    //    entity.FileName = customerPropertyFile.FileName;
                    //    entity.FileBlob = customerPropertyFile.FileBlob;
                    //    entity.UploadTime = DateTime.Now;
                    //    entity.FileLength = customerPropertyFile.FileBlob.Length;
                    //    entity.FileType = customerPropertyFile.FileType;
                    //    entity.PanID = customerPropertyFile.PanID;
                    //    entity.FileCategoryId = customerPropertyFile.FileCategoryId;
                    //    _context.CustomerPropertyFile.Update(entity);
                    //    await _context.SaveChangesAsync(cancellationToken);
                    //}
                }

                return true; 
            }
        }
    }
}
