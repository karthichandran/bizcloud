using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ReProServices.Application.Common.Interfaces;

namespace ReProServices.Application.LotSummaryReport.Query
{
    public class GetLotSummaryReportQuery : IRequest<IList<LotSummaryDto>>
    {
        public class GetLotSummaryReportQueryHandler : IRequestHandler<GetLotSummaryReportQuery, IList<LotSummaryDto>>
        {
            private readonly IApplicationDbContext _context;
            public GetLotSummaryReportQueryHandler(IApplicationDbContext context)
            {
                _context = context;
            }

            public async Task<IList<LotSummaryDto>> Handle(GetLotSummaryReportQuery request, CancellationToken cancellationToken)
            {
                var vm = await (from ls in _context.ViewLotSummary
                          select new LotSummaryDto
                          {
                              LotNo = ls.LotNo,
                              TotalPayments = ls.TotalPayments,
                              PaymentsConsidered = ls.PaymentsConsidered,
                              PaymentsNotConsidered = ls.PaymentsNotConsidered,
                              TransactionConsidered = ls.TransactionsConsidered,
                              TransactionNotConsidered = ls.TransactionsNotConsidered,
                              TransWithTdsPaid = ls.TransWithTdsPaid,
                              TransWithTdsPending = ls.TransWithTdsPending,
                              TransWithCoOwner = ls.TransWithCoOwner,
                              TransWithNoCoOwner = ls.TransWithNoCoOwner,
                              TransWithF16BGenerated = ls.TransWithF16BGenerated,
                              TransWithF16BGeneratedWithNotSharedOwnership = ls.TransWithF16BGeneratedWithNoSharedOwnership,
                              TransWithF16BGeneratedWithSharedOwnership = ls.TransWithF16BGeneratedWithSharedOwnership
                          }).ToListAsync(cancellationToken);

                return vm;
            }
        }
    }
}
