using MediatR;
using ReProServices.Application.Common.Interfaces;
using ReProServices.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ReProServices.Application.Receipts.Commands.UpdateReceipt
{
    public class UpdateEmailSentCommand : IRequest<int>
    {
        public int ReceiptId { get; set; }
        public class UpdateEmailSentCommandhandler : IRequestHandler<UpdateEmailSentCommand, int>
        {
            private readonly IApplicationDbContext _context;
            public UpdateEmailSentCommandhandler(IApplicationDbContext context)
            {
                _context = context;
            }

            public async Task<int> Handle(UpdateEmailSentCommand request, CancellationToken cancellationToken)
            {

              
                var entity = _context.Receipt
                   .First(x => x.ReceiptID == request.ReceiptId);
                entity.EmailSent = true;
                entity.EmailSentDate = DateTime.Now;              

                _context.Receipt.Update(entity);
                await _context.SaveChangesAsync(cancellationToken);
                return entity.ReceiptID;
            }
        }
    }
}
