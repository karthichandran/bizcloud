using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ReProServices.Application.Common.Interfaces;
using ReProServices.Application.Customers;
using ReProServices.Domain.Entities;

namespace ReProServices.Application.ClientPayments.Queries.ClientPaymentList
{
    public class GetClientPaymentListQuery : IRequest<CustomerVM>
    {
        public ClientPaymentFilter Filter { get; set; } = new ClientPaymentFilter();
        public class GetClientPaymentListQueryHandler : IRequestHandler<GetClientPaymentListQuery, CustomerVM>
        {
            private readonly IApplicationDbContext _context;
            private readonly IMapper _mapper;
            private readonly ILogger<GetClientPaymentListQueryHandler> _logger;
            public GetClientPaymentListQueryHandler(IApplicationDbContext context, IMapper mapper, 
                ILogger<GetClientPaymentListQueryHandler> logger)
            {
                _context = context;
                _mapper = mapper;
                _logger = logger;
            }

            public async Task<CustomerVM> Handle(GetClientPaymentListQuery request, CancellationToken cancellationToken)
            {
                CustomerVM customerVM = new CustomerVM();

                var cpList =  _context.ViewCustomerPropertyBasic
                    .PreFilterPaymentsListBy(request.Filter)
                    .Where(cp => cp.StatusTypeID <= 2)
                    .AsEnumerable()
                    .GroupBy(g => g.OwnershipID)
                    .Select(x => new ViewCustomerPropertyBasic
                    {
                        PropertyPremises = x.First().PropertyPremises,
                        CustomerName = string.Join(",", x.Select(g => g.CustomerName)),
                        PAN = string.Join(",", x.Select(g => g.PAN)),
                        DateOfAgreement = x.First().DateOfAgreement,
                        OwnershipID = x.First().OwnershipID,
                        UnitNo = x.First().UnitNo,
                        CustomerID = x.First().CustomerID,
                        CustomerPropertyID = x.First().CustomerPropertyID,
                        DateOfSubmission = x.First().DateOfSubmission,
                        PaymentMethodId = x.First().PaymentMethodId,
                        PropertyID = x.First().PropertyID,
                        Remarks = x.First().Remarks,
                        StatusTypeID = x.First().StatusTypeID,
                        TotalUnitCost = x.First().TotalUnitCost,
                        UnitStatus = x.First().UnitStatus.Trim()
                    })
                    .ToList()
                    .AsQueryable().PostFilterPaymentsListBy(request.Filter).ToList();
              
                customerVM.customersView = cpList;
                return customerVM;
            }
        }
    }
}

