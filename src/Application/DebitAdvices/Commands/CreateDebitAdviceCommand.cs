using MediatR;
using ReProServices.Application.Common.Interfaces;
using System.Threading.Tasks;
using System.Threading;
using ReProServices.Domain.Entities;
using System.Linq;
using System;
using ReProServices.Application.CustomerPropertyFiles;

namespace ReProServices.Application.DebitAdvices.Commands
{
   public class CreateDebitAdviceCommand:IRequest<int>
    {
        public DebitAdviceDto debitAdviceDto { get; set; }
        public class CreateDebitAdviceCommandHandler : IRequestHandler<CreateDebitAdviceCommand, int>
        {
            private readonly IApplicationDbContext _context;
            private readonly ICurrentUserService _currentUserService;
            public CreateDebitAdviceCommandHandler(IApplicationDbContext context, ICurrentUserService currentUserService)
            {
                _context = context;
                _currentUserService = currentUserService;
            }

            public async Task<int> Handle(CreateDebitAdviceCommand request, CancellationToken cancellationToken)
            {

                var obj = request.debitAdviceDto;
                var entity = new DebitAdvice
                {
                   ClientPaymentTransactionID=obj.ClientPaymentTransactionID,
                   CinNo=obj.CinNo,
                   PaymentDate=obj.PaymentDate,
                   BlobId=obj.BlobId
                };

                await _context.DebitAdvices.AddAsync(entity, cancellationToken);
                await _context.SaveChangesAsync(cancellationToken);
                return entity.DebitAdviceID;
            }
        }
    }
}
