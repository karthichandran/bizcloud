using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using ReProServices.Application.Common.Interfaces;
using ReProServices.Application.Customers;

namespace ReProServices.Application.CustomerProperty.Commands.UpdateCustomerProperty
{
    public class UpdateCustomerPropertyCommand : IRequest<CustomerVM>
    {
        public CustomerVM CustomerVM { get; set; }

        public class UpdateCustomerPropertyCommandHandler : IRequestHandler<UpdateCustomerPropertyCommand, CustomerVM>
        {
            private readonly IApplicationDbContext _context;
            private readonly ICurrentUserService _currentUserService;
            public UpdateCustomerPropertyCommandHandler(IApplicationDbContext context, ICurrentUserService currentUserService)
            {
                _context = context;
                _currentUserService = currentUserService;
            }

            public async Task<CustomerVM> Handle(UpdateCustomerPropertyCommand request, CancellationToken cancellationToken)
            {
                var userInfo = _context.Users.FirstOrDefault(x => x.UserName == _currentUserService.UserName && x.IsActive);
                foreach (var customer in request.CustomerVM.customers)
                {
                    foreach (var customerProperty in customer.CustomerProperty)
                    {
                        if (customerProperty.CustomerPropertyId <= 0)
                        { //throw new ApplicationException("missing customer property id");
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
                                StatusTypeId = customerProperty.StatusTypeId,
                            TdsCollectedBySeller = customerProperty.TdsCollectedBySeller,
                                TdsRateID = customerProperty.TdsRateID,
                                TotalUnitCost = customerProperty.TotalUnitCost,
                                UnitNo = customerProperty.UnitNo,
                                DateOfAgreement = customerProperty.DateOfAgreement?.Date,
                                //this Null-Collation will allow adding of a co-owner to an existing ownership
                                OwnershipID = customerProperty.OwnershipID ,
                                IsArchived = false,
                                StampDuty=customerProperty.StampDuty??0
                                //Created = DateTime.Now,
                                //CreatedBy = userInfo.UserID.ToString()
                            };

                            await _context.CustomerProperty.AddAsync(entity, cancellationToken);
                            await _context.SaveChangesAsync(cancellationToken);
                            customerProperty.CustomerPropertyId = entity.CustomerPropertyId;
                            customerProperty.OwnershipID = entity.OwnershipID;
                        }

                        var entity0 = await _context.CustomerProperty.FindAsync(customerProperty.CustomerPropertyId);

                        if (entity0 == null)
                        {
                            throw new ApplicationException($"The property {customerProperty.CustomerPropertyId} does not exist.");
                        }

                        entity0.CustomerPropertyId = customerProperty.CustomerPropertyId;
                        entity0.CustomerId = customerProperty.CustomerId;
                        entity0.CustomerShare = customerProperty.CustomerShare;
                        entity0.DateOfSubmission = customerProperty.DateOfSubmission.Date;
                        entity0.GstRateID = customerProperty.GstRateID;
                        entity0.IsShared = customerProperty.IsShared;
                        entity0.PaymentMethodId = customerProperty.PaymentMethodId;
                        entity0.PropertyId = customerProperty.PropertyId;
                        entity0.Remarks = customerProperty.Remarks;
                        entity0.StatusTypeId = customerProperty.StatusTypeId;
                        entity0.TdsCollectedBySeller = customerProperty.TdsCollectedBySeller;
                        entity0.TdsRateID = customerProperty.TdsRateID;
                        entity0.TotalUnitCost = customerProperty.TotalUnitCost;
                        entity0.UnitNo = customerProperty.UnitNo;
                        entity0.DateOfAgreement = customerProperty.DateOfAgreement?.Date;
                        entity0.CustomerAlias = customerProperty.CustomerAlias;
                        entity0.IsPrimaryOwner = customerProperty.IsPrimaryOwner;
                        entity0.StampDuty = customerProperty.StampDuty ?? 0;
                        //entity0.Updated = DateTime.Now;
                        //entity0.UpdatedBy = userInfo.UserID.ToString();
                        _context.CustomerProperty.Update(entity0);
                        await _context.SaveChangesAsync(cancellationToken);
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
