using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ReProServices.Application.Common.Interfaces;
using ReProServices.Domain.Enums;

namespace ReProServices.Application.Traces.Command
{
    public class DeleteTracesCommand : IRequest<Unit>
    {
        public int RemittanceID { get; set; }

        public class DeleteTracesCommandHandler : IRequestHandler<DeleteTracesCommand, Unit>
        {
            private readonly IApplicationDbContext _context;
            public DeleteTracesCommandHandler(IApplicationDbContext context)
            {
                _context = context;
            }
            public async Task<Unit> Handle(DeleteTracesCommand request, CancellationToken cancellationToken)
            {
                var remittance = _context.Remittance.First(x => x.RemittanceID == request.RemittanceID);

                if (remittance.ChallanBlobID != null)
                {
                    var challan = _context.CustomerPropertyFile.FirstOrDefault(x => x.BlobID == remittance.ChallanBlobID);
                    if (challan != null)
                    {
                        _ = _context.CustomerPropertyFile.Remove(challan).State = EntityState.Deleted;
                        await _context.SaveChangesAsync(cancellationToken);
                    }
                }

                if (remittance.Form16BlobID != null)
                {
                    var form16 = _context.CustomerPropertyFile.FirstOrDefault(x => x.BlobID == remittance.Form16BlobID);
                    _ = _context.CustomerPropertyFile.Remove(form16).State = EntityState.Deleted;
                    await _context.SaveChangesAsync(cancellationToken);
                }

                var cpt = _context.ClientPaymentTransaction
                                               .First(x => x.ClientPaymentTransactionID == remittance.ClientPaymentTransactionID);
                cpt.RemittanceStatusID = (int)ERemittanceStatus.Pending;
                _ = _context.ClientPaymentTransaction.Update(cpt).State = EntityState.Modified;
                await _context.SaveChangesAsync(cancellationToken);

                _ = _context.Remittance.Remove(remittance).State = EntityState.Deleted;
                await _context.SaveChangesAsync(cancellationToken);

                return Unit.Value;
            }
        }

    }
}
