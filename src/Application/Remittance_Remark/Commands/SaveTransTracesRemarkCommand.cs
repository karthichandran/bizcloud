using MediatR;
using ReProServices.Application.Common.Interfaces;
using System.Threading.Tasks;
using System.Threading;
using ReProServices.Domain.Entities;
using System.Linq;
using System;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;

namespace ReProServices.Application.Remittance_Remark.Commands
{
   public class SaveTransTracesRemarkCommand : IRequest<bool>
    {
        public int ClientPaymentTransactionId { get; set; }
        public int RemarkId { get; set; }

        public class SaveTransTracesRemarkCommandHandler : IRequestHandler<SaveTransTracesRemarkCommand, bool>
        {

            private readonly IApplicationDbContext _context;

            public SaveTransTracesRemarkCommandHandler(IApplicationDbContext context)
            {
                _context = context;
            }

            public async Task<bool> Handle(SaveTransTracesRemarkCommand request, CancellationToken cancellationToken)
            {
                try
                {
                    var entity = _context.ClientTransactionRemark.Where(x => x.ClientPaymentTransactionId == request.ClientPaymentTransactionId).FirstOrDefault();
                    if (entity == null)
                    {

                        entity = new ClientTransactionRemark
                        {
                            ClientPaymentTransactionId = request.ClientPaymentTransactionId,
                            TracesRemarkId = request.RemarkId
                        };

                        await _context.ClientTransactionRemark.AddAsync(entity, cancellationToken);
                    }
                    else
                    {
                        entity.TracesRemarkId = request.RemarkId;
                        _context.ClientTransactionRemark.Update(entity);
                    }
                    await _context.SaveChangesAsync(cancellationToken);

                    return true;
                }
                catch (Exception ex)
                {

                    throw ex;
                }


            }
        }
    }
}
