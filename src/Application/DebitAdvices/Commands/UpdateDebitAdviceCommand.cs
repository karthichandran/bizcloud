using MediatR;
using ReProServices.Application.Common.Interfaces;
using System.Threading.Tasks;
using System.Threading;
using ReProServices.Domain.Entities;
using System.Linq;

namespace ReProServices.Application.DebitAdvices.Commands
{
    public class UpdateDebitAdviceCommand : IRequest<int>
    {
        public DebitAdviceDto debitAdviceDto { get; set; }
        public class UpdateDebitAdviceCommandHandler : IRequestHandler<UpdateDebitAdviceCommand, int>
        {
            private readonly IApplicationDbContext _context;
            private readonly ICurrentUserService _currentUserService;
            public UpdateDebitAdviceCommandHandler(IApplicationDbContext context, ICurrentUserService currentUserService)
            {
                _context = context;
                _currentUserService = currentUserService;
            }

            public async Task<int> Handle(UpdateDebitAdviceCommand request, CancellationToken cancellationToken)
            {
                var model = request.debitAdviceDto;
                var entity = _context.DebitAdvices.FirstOrDefault(x => x.DebitAdviceID == model.DebitAdviceID);

                entity.CinNo = model.CinNo;
                entity.PaymentDate = model.PaymentDate;
                entity.BlobId = model.BlobId;                

                _context.DebitAdvices.Update(entity);
                await _context.SaveChangesAsync(cancellationToken);
                return entity.DebitAdviceID;
            }
        }
    }
}
