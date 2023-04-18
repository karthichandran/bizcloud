using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using ReProServices.Application.Common.Interfaces;
using Microsoft.EntityFrameworkCore;
using AutoMapper.QueryableExtensions;
using AutoMapper;
using ReProServices.Domain.Enums;

namespace ReProServices.Application.Receipts.Queries
{
    public class GetSavedReceiptsQuery : IRequest<IList<ReceiptDto>>
    {
        public ReceiptFilter Filter { get; set; } = new ReceiptFilter();
        public int ReceiptType { get; set; }
        public class GetSavedReceiptsQueryHandler : IRequestHandler<GetSavedReceiptsQuery, IList<ReceiptDto>>
        {
            private readonly IApplicationDbContext _context;
            private readonly IMapper _mapper;

            public GetSavedReceiptsQueryHandler(IApplicationDbContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            public async Task<IList<ReceiptDto>> Handle(GetSavedReceiptsQuery request, CancellationToken cancellationToken)
            {

                var rawList = new List<ReceiptDto>();

                //var vm = (from rec in _context.Receipt
                //          join cpt in _context.ClientPaymentTransaction on rec.ClientPaymentTransactionID equals cpt.ClientPaymentTransactionID
                //          join cp in _context.ViewCustomerPropertyExpanded on new { cpt.OwnershipID, cpt.CustomerID } equals new { cp.OwnershipID, cp.CustomerID }
                //          where rec.ReceiptType == 2
                //          select new ReceiptDto
                //          {
                //              ReceiptID = rec.ReceiptID,
                //              ClientPaymentTransactionID = cpt.ClientPaymentTransactionID,
                //              OwnershipID = cpt.OwnershipID,
                //              Tds = cpt.Tds,
                //              LotNo = rec.LotNo,
                //              CustomerName = cp.CustomerName,
                //              PropertyID = cp.PropertyID,
                //              SellerID = cpt.SellerID,
                //              UnitNo = cp.UnitNo,
                //              DateOfReceipt = rec.DateOfReceipt,
                //              ModeOfReceiptID = rec.ModeOfReceiptID,
                //              ReferenceNo = rec.ReferenceNo,
                //              TdsReceived = rec.TdsReceived
                //          })
                //                .PreFilterReceiptsBy(request.Filter)
                //                .ToList()
                //                .AsQueryable()
                //                .PostFilterReceiptsBy(request.Filter)
                //                .ToList();

                var vm = (from rec in _context.ViewReceipt
                          where rec.ReceiptType == request.ReceiptType
                          select new ReceiptDto
                          {
                              ReceiptID = rec.ReceiptID,
                              ClientPaymentTransactionID = rec.ClientPaymentTransactionID,
                              OwnershipID = rec.OwnershipID,
                              Tds = rec.Tds,
                              LotNo = rec.LotNo,
                              CustomerName = rec.CustomerName,
                              PropertyID = rec.PropertyID,
                              UnitNo = rec.UnitNo,
                              DateOfReceipt = rec.DateOfReceipt,
                              ModeOfReceiptID = rec.ModeOfReceiptID,
                              ReferenceNo = rec.ReferenceNo,
                              TdsReceived = rec.TdsReceived,
                              TotalServiceFeeReceived=rec.TotalServiceFeeReceived,
                              TdsInterestReceived=rec.TdsInterestReceived,
                              LateFeeReceived=rec.LateFeeReceived,
                              LateFee=rec.LateFee,
                              ServiceFee=rec.ServiceFee,
                              GstPayable=rec.GstPayable
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
