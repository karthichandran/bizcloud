using MediatR;
using ReProServices.Application.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;

namespace ReProServices.Application.DebitAdvices.Queries
{
    public class GetDebitAdviceByClientPaymentTransIdQuery : IRequest<DebitAdviceDto>
    {
        public int clientPaymentTransactionId { get; set; }
        public class GetDebitAdviceByClientPaymentTransIdQueryHandler : IRequestHandler<GetDebitAdviceByClientPaymentTransIdQuery, DebitAdviceDto>
        {
            private readonly IApplicationDbContext _context;
            public GetDebitAdviceByClientPaymentTransIdQueryHandler(IApplicationDbContext context)
            {
                _context = context;
            }

            public async Task<DebitAdviceDto> Handle(GetDebitAdviceByClientPaymentTransIdQuery request, CancellationToken cancellationToken)
            {

                var model = _context.DebitAdvices.Where(x => x.ClientPaymentTransactionID == request.clientPaymentTransactionId).Select(x=>new DebitAdviceDto { 
                DebitAdviceID=x.DebitAdviceID,
                ClientPaymentTransactionID=x.ClientPaymentTransactionID,
                PaymentDate=x.PaymentDate,
                CinNo=x.CinNo
                }).FirstOrDefault();

                return model;
               
            }
        }
    }
}
