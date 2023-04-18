using MediatR;
using ReProServices.Application.Common.Interfaces;
using System.Threading.Tasks;
using System.Threading;
using ReProServices.Application.Customers;
using System;
using ReProServices.Domain.Enums;
using System.Linq;

namespace ReProServices.Application.CustomerProperty.Commands.CreateCustomerProperty
{
    public class CreateCustomerPropertyCommand : IRequest<CustomerVM>
    {
        public CustomerVM CustomerVM { get; set; }

        public class CreateCustomerPropertyCommandHandler : IRequestHandler<CreateCustomerPropertyCommand, CustomerVM>
        {
            private readonly IApplicationDbContext _context;
            private readonly ICurrentUserService _currentUserService;
            public CreateCustomerPropertyCommandHandler(IApplicationDbContext context, ICurrentUserService currentUserService)
            {
                _context = context;
                _currentUserService = currentUserService;
            } 

            public async Task<CustomerVM> Handle(CreateCustomerPropertyCommand request, CancellationToken cancellationToken)
            {
                Guid guid = Guid.NewGuid();
                var userInfo = _context.Users.FirstOrDefault(x => x.UserName == _currentUserService.UserName && x.IsActive);
                //foreach (var customerProperty in request.CustomerVM.customerProperty)
                foreach (var customer in request.CustomerVM.customers)
                {
                    foreach (var customerProperty in customer.CustomerProperty)
                    {
                        if (customerProperty.CustomerPropertyId > 0) {
                            Domain.Entities.CustomerProperty entity = new Domain.Entities.CustomerProperty
                            {
                                CustomerId = customerProperty.CustomerId,
                                CustomerPropertyId = customerProperty.CustomerPropertyId,
                                CustomerShare = customerProperty.CustomerShare,
                                DateOfSubmission = customerProperty.DateOfSubmission.Date,
                                GstRateID = customerProperty.GstRateID,
                                IsShared = customerProperty.IsShared,
                                PaymentMethodId = customerProperty.PaymentMethodId,
                                PropertyId = customerProperty.PropertyId,
                                Remarks = customerProperty.Remarks,
                                StatusTypeId = (int)EStatusType.Saved,
                                TdsCollectedBySeller = customerProperty.TdsCollectedBySeller,
                                TdsRateID = customerProperty.TdsRateID,
                                TotalUnitCost = customerProperty.TotalUnitCost,
                                UnitNo = customerProperty.UnitNo,
                                DateOfAgreement = customerProperty.DateOfAgreement?.Date,
                                OwnershipID = customerProperty.OwnershipID,
                                CustomerAlias = customerProperty.CustomerAlias,
                                IsArchived = false,
                                StampDuty=customerProperty.StampDuty??0
                                //Updated = DateTime.Now,
                                //UpdatedBy = userInfo.UserID.ToString()

                            };

                            _context.CustomerProperty.Update(entity);
                            await _context.SaveChangesAsync(cancellationToken);
                            customerProperty.OwnershipID = entity.OwnershipID;
                        }
                        else
                        {

                            Domain.Entities.CustomerProperty entity = new Domain.Entities.CustomerProperty
                            {
                                CustomerId = customerProperty.CustomerId,
                                CustomerShare = customerProperty.CustomerShare,
                                DateOfSubmission = customerProperty.DateOfSubmission.Date,
                                GstRateID = customerProperty.GstRateID,
                                IsShared = customerProperty.IsShared,
                                PaymentMethodId = customerProperty.PaymentMethodId,
                                PropertyId = customerProperty.PropertyId,
                                Remarks = customerProperty.Remarks,
                                StatusTypeId = (int)EStatusType.Saved,
                                TdsCollectedBySeller = customerProperty.TdsCollectedBySeller,
                                TdsRateID = customerProperty.TdsRateID,
                                TotalUnitCost = customerProperty.TotalUnitCost,
                                UnitNo = customerProperty.UnitNo,
                                DateOfAgreement = customerProperty.DateOfAgreement?.Date,
                                //this Null-Collation will allow adding of a co-owner to an existing ownership
                                OwnershipID = customerProperty.OwnershipID ?? guid,
                                IsArchived = false,
                                StampDuty = customerProperty.StampDuty??0
                                //Created = DateTime.Now,
                                //CreatedBy = userInfo.UserID.ToString()
                            };

                            await _context.CustomerProperty.AddAsync(entity, cancellationToken);
                            await _context.SaveChangesAsync(cancellationToken);
                            customerProperty.CustomerPropertyId = entity.CustomerPropertyId;
                            customerProperty.OwnershipID = entity.OwnershipID;
                        }
                    }
                    
                }

                CustomerVM customerVM = new CustomerVM
                {
                    customers = request.CustomerVM.customers
                };
                return customerVM;
            }
        }
    }
}
