using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using ReProServices.Application.Common.Interfaces;

namespace ReProServices.Application.Receipts.Queries
{
    public class GetCoOwnersByReceiptIdQuery: IRequest<IList<CoOwnersDto>>
    {
        public int ReceiptId { get; set; } = 0;
        public class GetCoOwnersByReceiptIdQueryHandler : IRequestHandler<GetCoOwnersByReceiptIdQuery, IList<CoOwnersDto>>
        {
            private readonly IApplicationDbContext _context;
            public GetCoOwnersByReceiptIdQueryHandler(IApplicationDbContext context)
            {
                _context = context;
            }

            public async Task<IList<CoOwnersDto>> Handle(GetCoOwnersByReceiptIdQuery request, CancellationToken cancellationToken)
            {
                var rawList = new List<ReceiptDto>();

                var customers = await (from c in _context.Customer
                          join cp in _context.CustomerProperty on c.CustomerID equals cp.CustomerId
                          join rp in _context.Receipt on cp.OwnershipID equals rp.OwnershipID
                          where rp.ReceiptID == request.ReceiptId
                          select new CoOwnersDto
                          {
                              CustomerName = c.Name,
                              Email = c.EmailID,
                              isprimaryOwner = cp.IsPrimaryOwner
                          }).ToListAsync(cancellationToken);                
                return customers;

            }
        }
    }
}
