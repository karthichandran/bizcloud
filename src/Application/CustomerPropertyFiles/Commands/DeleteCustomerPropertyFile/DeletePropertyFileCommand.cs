using MediatR;
using ReProServices.Application.Common.Interfaces;
using System.Threading.Tasks;
using System.Threading;
using System.Linq;

namespace ReProServices.Application.CustomerPropertyFiles.Commands.DeleteCustomerPropertyFile
{
    public class DeletePropertyFileCommand : IRequest<bool> //todo send an improved response to client
    {
        public int BlobID { get; set; }

        public class DeletePropertyFileCommandHandler : IRequestHandler<DeletePropertyFileCommand, bool>
        {
            private readonly IApplicationDbContext _context;
            public DeletePropertyFileCommandHandler(IApplicationDbContext context)
            {
                _context = context;
            }

            public async Task<bool> Handle(DeletePropertyFileCommand request, CancellationToken cancellationToken)
            {
                var fileObj = _context.CustomerPropertyFile
                        .First(x => x.BlobID == request.BlobID);
                _context.CustomerPropertyFile.Remove(fileObj);
                await _context.SaveChangesAsync(cancellationToken);


                return true; //todo fix this broken window
            }
        }
    }
}
