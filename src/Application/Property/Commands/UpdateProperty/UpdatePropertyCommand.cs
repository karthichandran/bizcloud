using System;
using System.Linq;
using MediatR;
using ReProServices.Application.Common.Interfaces;
using System.Threading.Tasks;
using System.Threading;
using ReProServices.Application.Common.Exceptions;
using ReProServices.Domain.Entities;

namespace ReProServices.Application.Property.Commands.UpdateProperty
{
    public class UpdatePropertyCommand : IRequest<int>
    {
        public PropertyVM PropertyVM { get; set; }
        public class UpdatePropertyCommandHandler : IRequestHandler<UpdatePropertyCommand, int>
        {
            private readonly IApplicationDbContext _context;
            private readonly ICurrentUserService _currentUserService;
            public UpdatePropertyCommandHandler(IApplicationDbContext context, ICurrentUserService currentUserService)
            {
                _context = context;
                _currentUserService = currentUserService;
            }

            public async Task<int> Handle(UpdatePropertyCommand request, CancellationToken cancellationToken)
            {
                var userInfo = _context.Users.FirstOrDefault(x => x.UserName == _currentUserService.UserName && x.IsActive);
                var entity = await _context.Property.FindAsync(request.PropertyVM.propertyDto.PropertyID);

                if (entity == null)
                {
                    throw new ApplicationException($"The property {request.PropertyVM.propertyDto.PropertyID} does not exist.");
                }

                var propertyObj = request.PropertyVM.propertyDto;
                entity.PropertyType = propertyObj.PropertyType;
                entity.AddressPremises = propertyObj.AddressPremises;
                entity.AddressLine1 = propertyObj.AddressLine1;
                entity.AddressLine2 = propertyObj.AddressLine2;
                entity.City = propertyObj.City;
                entity.PinCode = propertyObj.PinCode.Trim();
                entity.LateFeePerDay = propertyObj.LateFeePerDay;
                entity.TdsInterestRate = propertyObj.TdsInterestRate;
                entity.StateID = propertyObj.StateID;
                entity.GstTaxCode = propertyObj.GstTaxCode;
                entity.TDSTaxCode = propertyObj.TDSTaxCode;
                entity.PropertyCode = propertyObj.PropertyCode;
                entity.PropertyShortName = propertyObj.PropertyShortName;
                entity.IsActive= propertyObj.IsActive;
                entity.Updated = DateTime.Now;
                entity.UpdatedBy = userInfo.UserID.ToString();
                _context.Property.Update(entity);
                await _context.SaveChangesAsync(cancellationToken);

                var shareTotal = request.PropertyVM.sellerProperties
                                               .Where(d => d.MarkForDelete == false)
                                               .Sum(x => x.SellerShare);
                if (shareTotal != 100)
                {
                    throw new System.ApplicationException("Total Share value is not 100");
                }

                foreach (var sellerProp in request.PropertyVM.sellerProperties)
                {
                    if (sellerProp.MarkForDelete)
                    {
                        var forDel =
                            _context.SellerProperty.First(x => x.SellerPropertyID == sellerProp.SellerPropertyId);
                        _ = _context.SellerProperty.Remove(forDel);
                        _ = await _context.SaveChangesAsync(cancellationToken);
                        continue;
                    }


                    if (sellerProp.SellerPropertyId <= 0 )
                    {
                        SellerProperty sellerPropObj1 = new SellerProperty
                        {
                            PropertyID = entity.PropertyID,
                            SellerID = sellerProp.SellerID,
                            SellerShare = sellerProp.SellerShare,
                            isSharedSeller=sellerProp.IsSharedSeller,
                            Created = DateTime.Now,
                            CreatedBy = userInfo.UserID.ToString()
                        };
                        _ = await _context.SellerProperty.AddAsync(sellerPropObj1, cancellationToken);
                        _ = await _context.SaveChangesAsync(cancellationToken);
                        continue;
                    }

                    SellerProperty sellerPropObj = new SellerProperty
                    {
                        SellerPropertyID = sellerProp.SellerPropertyId,
                        PropertyID = entity.PropertyID,
                        SellerID = sellerProp.SellerID,
                        SellerShare = sellerProp.SellerShare,
                        isSharedSeller=sellerProp.IsSharedSeller,
                        Updated=DateTime.Now,
                        UpdatedBy= userInfo.UserID.ToString()
                    };
                    _ = _context.SellerProperty.Update(sellerPropObj);
                    _ = await _context.SaveChangesAsync(cancellationToken);
                }

                return entity.PropertyID;

            }
        }
    }
}
