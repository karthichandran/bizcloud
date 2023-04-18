using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using ReProServices.Application.Common.Interfaces;

namespace ReProServices.Application.Prospect.Command
{
    public class DeleteProspectAndPropertyCommand : IRequest<Unit>
    {
        public int prospectPropertyID { get; set; }
        public class DeleteProspectAndPropertyCommandHandler : IRequestHandler<DeleteProspectAndPropertyCommand, Unit> {
            private readonly IApplicationDbContext _context;
            public DeleteProspectAndPropertyCommandHandler(IApplicationDbContext context)
            {
                _context = context;
            }
            public async Task<Unit> Handle(DeleteProspectAndPropertyCommand request, CancellationToken cancellationToken)
            {

                var prospects = _context.Prospect.Where(x => x.ProspectPropertyID ==request.prospectPropertyID).ToList();
                if(prospects.Count==0)
                    throw new ApplicationException("PreSales records are not available" );

                _context.Prospect.RemoveRange(prospects);
                await _context.SaveChangesAsync(cancellationToken);

                var prospectProperty = _context.ProspectProperty.First(x => x.ProspectPropertyID== request.prospectPropertyID);
                if (prospectProperty==null)
                    throw new ApplicationException("PreSales records are not available");

                _context.ProspectProperty.Remove(prospectProperty);
                await _context.SaveChangesAsync(cancellationToken);

                return Unit.Value;
            }
        }
    }
}
