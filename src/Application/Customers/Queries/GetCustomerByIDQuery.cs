using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ReProServices.Application.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ReProServices.Application.Customers.Queries
{
    public class GetCustomerByIDQuery : IRequest<CustomerVM>
    {
        public Guid OwnershipId { get; set; }
        public class GetCustomersByIDQueryHandler : IRequestHandler<GetCustomerByIDQuery, CustomerVM>
        {
            private readonly IApplicationDbContext _context;
            private readonly IMapper _mapper;
            public GetCustomersByIDQueryHandler(IApplicationDbContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            public async Task<CustomerVM> Handle(GetCustomerByIDQuery request, CancellationToken cancellationToken)
            {

                CustomerVM customerVM = new CustomerVM();
                var vm = await _context.Customer
                    .Include(cp => cp.CustomerProperty)
                    .Where(cp => cp.CustomerProperty.Any(c => c.OwnershipID == request.OwnershipId))
                    .ProjectTo<CustomerDto>(_mapper.ConfigurationProvider)
                    .ToListAsync(cancellationToken);

                string sellersName="" ;
                if (vm.Count > 0) {
                    if (vm[0].CustomerProperty.Count > 0) {
                        var propID = vm[0].CustomerProperty.ToList()[0].PropertyId;
                        var sellerDto = (from seller in _context.Seller
                                         join sp in _context.SellerProperty on seller.SellerID equals sp.SellerID
                                         where sp.PropertyID == propID
                                         select seller);
                        sellersName = string.Join(",", sellerDto.Select(s => s.SellerName));
                    }
                }

                var vmResult = new List<CustomerDto>();
                foreach (var custVm in vm)
                {
                    if (custVm.CustomerProperty.Count == 0)
                        vmResult.Add(custVm);
                    else
                        foreach (var custProp in custVm.CustomerProperty)
                        {
                            if (custProp.OwnershipID != request.OwnershipId)
                                continue;
                            custProp.Property = null; //class is not consumed in ui
                            var dto = new CustomerDto
                            {
                                AddressLine2 = custVm.AddressLine2,
                                AddressPremises = custVm.AddressPremises,
                                AllowForm16B = custVm.AllowForm16B,
                                AdressLine1 = custVm.AdressLine1,
                                City = custVm.City,
                                CustomerID = custVm.CustomerID,
                                DateOfBirth = custVm.DateOfBirth.Date,
                                EmailID = custVm.EmailID,
                                IsTracesRegistered = custVm.IsTracesRegistered,
                                MobileNo = custVm.MobileNo,
                                Name = custVm.Name,
                                PinCode = custVm.PinCode,
                                PAN = custVm.PAN,
                                StateId = custVm.StateId,
                                TracesPassword = custVm.TracesPassword,
                                AlternateNumber = custVm.AlternateNumber,
                                ISD = custVm.ISD,
                                Sellers = sellersName,
                                CustomerProperty = new List<Domain.Entities.CustomerProperty>() { custProp },
                                IsPanVerified = custVm.IsPanVerified == null ? false : custVm.IsPanVerified,
                                OnlyTDS = custVm.OnlyTDS,
                                IncomeTaxPassword=custVm.IncomeTaxPassword
                            };
                            vmResult.Add(dto);
                        }
                }
                customerVM.customers = vmResult;



               // customerVM.customers = vm;
                return customerVM;
            }
        }
    }
}
