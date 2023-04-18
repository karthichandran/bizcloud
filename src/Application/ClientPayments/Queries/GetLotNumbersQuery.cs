using MediatR;
using ReProServices.Application.Common.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using System;
using System.Collections.Generic;
using ReProServices.Domain.Extensions;

namespace ReProServices.Application.ClientPayments.Queries
{
    public class GetLotNumbersQuery:  IRequest<IList<LotNumbersDto>>
    {
        public class GetLotNumbersQueryHandler :
                            IRequestHandler<GetLotNumbersQuery, IList<LotNumbersDto>>
        {

            private readonly IApplicationDbContext _context;

            public GetLotNumbersQueryHandler(IApplicationDbContext context)
            {
                _context = context;
            }

            public async Task<IList<LotNumbersDto>> Handle(GetLotNumbersQuery request, CancellationToken cancellationToken)
            {

                var vm = await (from cp in _context.ClientPayment
                          select new LotNumbersDto
                          {
                              LotNo = cp.LotNo
                          }).Distinct().ToListAsync(cancellationToken: cancellationToken);
                return vm;
            }

        }
    }
}
