using System;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using MediatR;
using ReProServices.Application.Common.Interfaces;
using ReProServices.Domain;
using ReProServices.Domain.Entities;

namespace ReProServices.Application.User.Command
{
    public class CreateUserCommand : IRequest<int>
    {
        public UserVM UserVM{ get; set; }

        public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, int> {
            private readonly IApplicationDbContext _context;
            private readonly ICurrentUserService _currentUserService;
            public CreateUserCommandHandler(IApplicationDbContext context, ICurrentUserService currentUserService)
            {
                _context = context;
                _currentUserService = currentUserService;
            }
            public async Task<int> Handle(CreateUserCommand request, CancellationToken cancellationToken)
            {                
                var userDto = request.UserVM.userDto;

                var existUser = _context.Users.FirstOrDefault(x => x.LoginName == userDto.LoginName);
                if (existUser != null) {
                    throw new ApplicationException("The login Name is already exists");
                }

                var loggedUserInfo = _context.Users.FirstOrDefault(x => x.UserName == _currentUserService.UserName && x.IsActive);

                Domain.Entities.Users entity = new Domain.Entities.Users
                {
                    UserName= userDto.UserName,
                    LoginName= userDto.LoginName,
                    UserPassword= userDto.UserPassword,
                    Code= userDto.Code,
                    Email= userDto.Email,
                    MobileNo= userDto.MobileNo,
                    GenderID= userDto.GenderID,
                    DateOfBirth= userDto.DateOfBirth,
                    FromDate= userDto.FromDate,
                    ToDate= userDto.ToDate,
                    IsActive= userDto.IsActive,
                    IsAgent= userDto.IsAgent,
                    ISD= userDto.ISD,
                    IsAdmin=userDto.IsAdmin,
                    //Created = DateTime.Now,
                  //  CreatedBy = loggedUserInfo.UserID.ToString()
                };
                await _context.Users.AddAsync(entity, cancellationToken);
                await _context.SaveChangesAsync(cancellationToken);

                var rolesList = request.UserVM.userRolesDto;
                foreach (var role in rolesList) {
                    var roleEntity = new UserRoles
                    {
                        UserID = entity.UserID,
                        RoleID = role.RoleID
                        //Created = DateTime.Now,
                        //CreatedBy = loggedUserInfo.UserID.ToString()
                    };
                  
                    await _context.UserRoles.AddAsync(roleEntity, cancellationToken);
                }
                await _context.SaveChangesAsync(cancellationToken);

                return entity.UserID;
            }

            }
    }
}
