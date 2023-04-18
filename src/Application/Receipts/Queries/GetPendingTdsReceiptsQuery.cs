using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using ReProServices.Application.Common.Interfaces;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using AutoMapper.QueryableExtensions;

namespace ReProServices.Application.Receipts.Queries
{
    public class GetPendingTdsReceiptsQuery : IRequest<IList<ReceiptDto>>
    {
        public ReceiptFilter Filter { get; set; } = new ReceiptFilter();
        public class GetReceiptsQueryHandler : IRequestHandler<GetPendingTdsReceiptsQuery, IList<ReceiptDto>>
        {
            private readonly IApplicationDbContext _context;
            private readonly IMapper _mapper;
            public GetReceiptsQueryHandler(IApplicationDbContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            public async Task<IList<ReceiptDto>> Handle(GetPendingTdsReceiptsQuery request, CancellationToken cancellationToken)
            {
                var rawList = new List<ReceiptDto>();
                var exceptionList = await _context.Receipt
                    .Where(x => x.ReceiptType == 2)
                    .ProjectTo<ReceiptDto>(_mapper.ConfigurationProvider).ToListAsync(cancellationToken);


                var vm = (from pay in _context.ClientPayment
                          join cpt in _context.ClientPaymentTransaction on new { pay.InstallmentID, pay.ClientPaymentID } equals new { cpt.InstallmentID, cpt.ClientPaymentID }
                          join cp in _context.ViewCustomerPropertyExpanded on new { cpt.OwnershipID, cpt.CustomerID } equals new { cp.OwnershipID, cp.CustomerID }
                          where pay.NatureOfPaymentID == (int)ReProServices.Domain.Enums.ENatureOfPayment.ToBeConsidered
                          select new ReceiptDto
                          {
                              ClientPaymentTransactionID = cpt.ClientPaymentTransactionID,
                              OwnershipID = cpt.OwnershipID,
                              Tds = cpt.Tds,
                              LotNo = pay.LotNo,
                              CustomerName = cp.CustomerName,
                              PropertyID = cp.PropertyID,
                              SellerID = cpt.SellerID,
                              UnitNo = cp.UnitNo,
                              CustomerID = cp.CustomerID
                          })
                                .PreFilterReceiptsBy(request.Filter)
                                .ToList()
                                .Where(x => exceptionList.All(y => y.ClientPaymentTransactionID != x.ClientPaymentTransactionID))
                                .AsQueryable()
                                .PostFilterReceiptsBy(request.Filter)
                                .ToList();
                return vm;
            }
        }
    }
}
