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
    public class UpdateGoogelDriveFileIdCommand : IRequest<bool> //todo send an improved response to client
    {
        public int blobId { get; set; }
        public string gdId { get; set; }

        public class UpdateGoogelDriveFileIdCommandHandler : IRequestHandler<UpdateGoogelDriveFileIdCommand, bool>
        {
            private readonly IApplicationDbContext _context;
            public UpdateGoogelDriveFileIdCommandHandler(IApplicationDbContext context)
            {
                _context = context;
            }

            public async Task<bool> Handle(UpdateGoogelDriveFileIdCommand request, CancellationToken cancellationToken)
            {

                var entity =  _context.CustomerPropertyFile.FirstOrDefault(z => z.BlobID == request.blobId);

                entity.GDfileID = request.gdId;
                 _context.CustomerPropertyFile.Update(entity);
                await _context.SaveChangesAsync(cancellationToken);

                return true;
            }
        }
    }
}
