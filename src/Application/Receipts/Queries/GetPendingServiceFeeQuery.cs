using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using ReProServices.Application.Common.Interfaces;

namespace ReProServices.Application.Receipts.Queries
{
   public class GetPendingServiceFeeQuery: IRequest<IList<ReceiptDto>>
    {
        public ReceiptFilter Filter { get; set; } = new ReceiptFilter();
    public class GetPendingServiceFeeQueryHandler : IRequestHandler<GetPendingServiceFeeQuery, IList<ReceiptDto>>
    {
        private readonly IApplicationDbContext _context;
        public GetPendingServiceFeeQueryHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IList<ReceiptDto>> Handle(GetPendingServiceFeeQuery request, CancellationToken cancellationToken)
        {
            var rawList = new List<ReceiptDto>();

            var vm = (from  psf in _context.ViewPendingServiceFee 
                      where (request.Filter.PropertyID > 0) ? psf.PropertyID == request.Filter.PropertyID : true
                      select new ReceiptDto
                      {
                          OwnershipID = psf.OwnershipID,
                          CustomerName =psf.ClientName,
                          UnitNo = psf.UnitNo.Value,
                          CustomerBillingID = psf.CustomerBillID,
                          ServiceFee = psf.ServiceFee.Value,
                          GstPayable = psf.GstPayable.Value,                          
                          TotalServiceFeeReceived = psf.TotalPayable.Value,
                          TdsInterest = psf.TdsInterest.Value,
                          LateFee = psf.LateFee.Value,
                          CustomerID = psf.CustomerID
                      })
                            .PreFilterReceiptsBy(request.Filter)
                            .ToList()
                            .AsQueryable()
                            .PostFilterReceiptsBy(request.Filter)
                            .ToList();

          
            return vm;

        }
    }
}
}
