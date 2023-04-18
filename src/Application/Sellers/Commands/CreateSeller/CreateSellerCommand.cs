using MediatR;
using ReProServices.Application.Common.Interfaces;
using System.Threading.Tasks;
using System.Threading;
using ReProServices.Domain.Entities;
using System.Linq;
using System;

namespace ReProServices.Application.Sellers.Commands.CreateSeller
{
    public class CreateSellerCommand : IRequest<int>
    {
        public SellerDto SellerDtoObj { get; set; }

        public class CreateSellerCommandHandler : IRequestHandler<CreateSellerCommand, int>
        {
            private readonly IApplicationDbContext _context;
            private readonly ICurrentUserService _currentUserService;
            public CreateSellerCommandHandler(IApplicationDbContext context, ICurrentUserService currentUserService)
            {
                _context = context;
                _currentUserService = currentUserService;
            }

            public async Task<int> Handle(CreateSellerCommand request, CancellationToken cancellationToken)
            {
                var sellerObj = request.SellerDtoObj;
                var userInfo = _context.Users.FirstOrDefault(x => x.UserName == _currentUserService.UserName && x.IsActive);
                Seller seller = new Seller
                {
                    AdressLine1 = sellerObj.AdressLine1==null?"": sellerObj.AdressLine1,
                    AddressLine2 = sellerObj.AddressLine2,
                    AddressPremises = sellerObj.AddressPremises,
                    City = sellerObj.City,
                    EmailID = sellerObj.EmailID,
                    MobileNo = sellerObj.MobileNo,
                    PAN = sellerObj.PAN,
                    PinCode = sellerObj.PinCode.Trim(),
                    SellerName = sellerObj.SellerName,
                    StateID = sellerObj.StateID,
                    IsResident = sellerObj.IsResident,
                    Created = DateTime.Now,
                    CreatedBy = userInfo.UserID.ToString()
                };

                _= await _context.Seller.AddAsync(seller, cancellationToken);
                await _context.SaveChangesAsync(cancellationToken);
                return seller.SellerID;

            }
        }
        

    }
}
