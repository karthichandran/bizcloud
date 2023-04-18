using MediatR;
using ReProServices.Application.Common.Interfaces;
using ReProServices.Domain.Entities;
using ReProServices.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
namespace ReProServices.Application.Remittances.Commands.UpdateRemittance
{
    public class UpdateEmailSentCommand : IRequest<int>
    {
        public int RemittanceID { get; set; }

        public class UpdateEmailSentCommandhandler : IRequestHandler<UpdateEmailSentCommand, int>
        {
            private readonly IApplicationDbContext _context;
            public UpdateEmailSentCommandhandler(IApplicationDbContext context)
            {
                _context = context;
            }

            public async Task<int> Handle(UpdateEmailSentCommand request, CancellationToken cancellationToken)
            {
                var entity = _context.Remittance
                   .First(x => x.RemittanceID == request.RemittanceID);
                entity.EmailSent = true;
                entity.EmailSentDate = DateTime.Now;

                var CptEntity = _context.ClientPaymentTransaction
                    .First(x => x.ClientPaymentTransactionID == entity.ClientPaymentTransactionID);
                CptEntity.RemittanceStatusID = (int)ERemittanceStatus.Form16BSentToCustomer;

                _context.Remittance.Update(entity);
                _context.ClientPaymentTransaction.Update(CptEntity);
                await _context.SaveChangesAsync(cancellationToken);
                return entity.RemittanceID;
            }
        }
    }
}
