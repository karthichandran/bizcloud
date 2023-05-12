using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ReProServices.Application.ClientPayments;
using ReProServices.Application.Common.Interfaces;

namespace ReProServices.Application.Remittances.Queries
{
    public class GetRemittancesQuery : IRequest<RemittanceDto>
    {
        public int ClientPaymentTransactionID { get; set; }
        public class GetRemittancesQueryHandler : IRequestHandler<GetRemittancesQuery, RemittanceDto>
        {
            private readonly IApplicationDbContext _context;
            private readonly IMapper _mapper;

            public GetRemittancesQueryHandler(IApplicationDbContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            public async Task<RemittanceDto> Handle(GetRemittancesQuery request, CancellationToken cancellationToken)
            {
                var vm1 = _context.Remittance
                .FirstOrDefault(_ => _.ClientPaymentTransactionID == request.ClientPaymentTransactionID);

                var vm = await _context.Remittance
                    .ProjectTo<RemittanceDto>(_mapper.ConfigurationProvider)
                    .FirstOrDefaultAsync(_ => _.ClientPaymentTransactionID == request.ClientPaymentTransactionID,
                                        cancellationToken: cancellationToken);

                if (vm == null)
                {
                    vm = new RemittanceDto();
                }

                var det = await (from pay in _context.ClientPayment
                                 join cpt in _context.ClientPaymentTransaction on new { pay.InstallmentID, pay.ClientPaymentID }
                                                                           equals new { cpt.InstallmentID, cpt.ClientPaymentID }
                                 join cp in _context.ViewCustomerPropertyExpanded on new { cpt.OwnershipID, cpt.CustomerID }
                                                                              equals new { cp.OwnershipID, cp.CustomerID }
                                 where cpt.ClientPaymentTransactionID == request.ClientPaymentTransactionID
                                 select new ClientPaymentRawDto
                                 {
                                     PropertyPremises = cp.PropertyPremises,
                                     LotNo = pay.LotNo,
                                     CustomerName = cp.CustomerName,
                                     UnitNo = cp.UnitNo,
                                     DateOfBirth = cp.DateOfBirth.Date,
                                     PAN = cp.CustomerPAN,

                                 }).FirstOrDefaultAsync(cancellationToken);
                var da = _context.DebitAdvices.Where(x => x.ClientPaymentTransactionID == request.ClientPaymentTransactionID).FirstOrDefault();

                vm.UnitNo = det.UnitNo;
                vm.LotNo = det.LotNo;
                vm.CustomerName = det.CustomerName;
                vm.Premises = det.PropertyPremises;
                vm.DateOfBirth = det.DateOfBirth;
                vm.CustomerPAN = det.PAN;
                if (da != null)
                {
                    vm.DebitAdviceID = da.DebitAdviceID;
                    vm.CinNo = da.CinNo;
                    vm.PaymentDate = da.PaymentDate;
                    vm.DebitAdviceBlobId = da.BlobId;
                }
                return vm;
            }
        }
    }
}
