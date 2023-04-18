using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using ReProServices.Application.Common.Interfaces;
using ReProServices.Domain;
using ReProServices.Domain.Entities;

namespace ReProServices.Application.Property.Commands.CreateProperty
{
    public class CreatePropertyCommand : IRequest<PropertyVM>
    {
        public PropertyVM PropertyVM { get; set; }

        public class CreatePropertyCommandHandler : IRequestHandler<CreatePropertyCommand, PropertyVM>
        {
            private readonly IApplicationDbContext _context;
            private readonly ICurrentUserService _currentUserService;
            public CreatePropertyCommandHandler(IApplicationDbContext context, ICurrentUserService currentUserService)
            {
                _context = context;
                _currentUserService = currentUserService;
            }

            public async Task<PropertyVM> Handle(CreatePropertyCommand request, CancellationToken cancellationToken)
            {
                decimal sum = 0;
                foreach (var sellerObj in request.PropertyVM.sellerProperties)
                { 
                    sum += sellerObj.SellerShare;
                }
                if (Convert.ToInt32(sum) != 100 && Convert.ToInt32(sum) >0)
                {
                    throw new DomainException("Seller Share is not 100");
                }

                var userInfo = _context.Users.FirstOrDefault(x => x.UserName == _currentUserService.UserName && x.IsActive);
                var propertyObj = request.PropertyVM.propertyDto;
                Domain.Entities.Property property = new Domain.Entities.Property
                {
                    PropertyType = propertyObj.PropertyType,
                    AddressPremises = propertyObj.AddressPremises,
                    AddressLine1 = propertyObj.AddressLine1,
                    AddressLine2 = propertyObj.AddressLine2,
                    City = propertyObj.City.Trim(),
                    PinCode = propertyObj.PinCode.Trim(),
                    LateFeePerDay = propertyObj.LateFeePerDay,
                    TdsInterestRate = propertyObj.TdsInterestRate,
                    StateID = propertyObj.StateID,
                    GstTaxCode = propertyObj.GstTaxCode,
                    TDSTaxCode = propertyObj.TDSTaxCode,
                    PropertyCode=propertyObj.PropertyCode,
                    PropertyShortName=propertyObj.PropertyShortName,
                    IsActive=propertyObj.IsActive,
                    Created = DateTime.Now,
                    CreatedBy = userInfo.UserID.ToString()
                };

                await _context.Property.AddAsync(property, cancellationToken);
                await _context.SaveChangesAsync(cancellationToken);
                propertyObj.PropertyID = property.PropertyID;

                foreach (var sellerProp in request.PropertyVM.sellerProperties)
                {
                    SellerProperty sellerPropObj = new SellerProperty
                    {
                        PropertyID = property.PropertyID,
                        SellerID = sellerProp.SellerID,
                        SellerShare = sellerProp.SellerShare,
                        isSharedSeller=sellerProp.IsSharedSeller,
                        Created = DateTime.Now,
                        CreatedBy = userInfo.UserID.ToString()
                    };
                    await _context.SellerProperty.AddAsync(sellerPropObj, cancellationToken);
                    await _context.SaveChangesAsync(cancellationToken);
                }

                return request.PropertyVM;
            }
        }

    }
}
