using MediatR;
using ReProServices.Application.Common.Interfaces;
using System.Threading.Tasks;
using System.Threading;
using ReProServices.Domain;
using System.Linq;
using System;

namespace ReProServices.Application.Sellers.Commands.UpdateSeller
{
    public class UpdateSellerCommand : IRequest<int>
    {
        public SellerDto SellerDto { get; set; }

        public class UpdateSellerCommandHandler : IRequestHandler<UpdateSellerCommand, int>
        {
            private readonly IApplicationDbContext _context;
            private readonly ICurrentUserService _currentUserService;
            public UpdateSellerCommandHandler(IApplicationDbContext context, ICurrentUserService currentUserService)
            {
                _context = context;
                _currentUserService = currentUserService;
            }

            public async Task<int> Handle(UpdateSellerCommand request, CancellationToken cancellationToken)
            {
                var userInfo = _context.Users.FirstOrDefault(x => x.UserName == _currentUserService.UserName && x.IsActive);
                var entity = await _context.Seller.FindAsync(request.SellerDto.SellerID);

                if (entity == null)
                {
                    throw new DomainException("Seller Id  does not exist");
                }
                var sellerObj = request.SellerDto;
                entity.AdressLine1 = sellerObj.AdressLine1;
                entity.AddressLine2 = sellerObj.AddressLine2;
                entity.AddressPremises = sellerObj.AddressPremises;
                entity.City = sellerObj.City;
                entity.EmailID = sellerObj.EmailID;
                entity.MobileNo = sellerObj.MobileNo;
                entity.PAN = sellerObj.PAN;
                entity.PinCode = sellerObj.PinCode.Trim();
                entity.SellerName = sellerObj.SellerName;
                entity.StateID = sellerObj.StateID;
                entity.IsResident = sellerObj.IsResident;
                entity.Updated = DateTime.Now;
                entity.UpdatedBy = userInfo.UserID.ToString();
                _context.Seller.Update(entity);
                await _context.SaveChangesAsync(cancellationToken);
                return entity.SellerID;

            }
        }
    }
}
