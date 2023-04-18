using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using ReProServices.Application.Common.Interfaces;
using ReProServices.Domain;
using ReProServices.Domain.Entities;

namespace ReProServices.Application.User.Command
{
    public class UpdateUserCommand : IRequest<int>
    {
        public UserVM UserVM { get; set; }

        public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, int>
        {
            private readonly IApplicationDbContext _context;
            private readonly ICurrentUserService _currentUserService;
            public UpdateUserCommandHandler(IApplicationDbContext context, ICurrentUserService currentUserService)
            {
                _context = context;
                _currentUserService = currentUserService;
            }
            public async Task<int> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
            {
                var dto = request.UserVM.userDto;
                var entity = _context.Users.FirstOrDefault(x=>x.UserID== dto.UserID);

                if (entity == null) {
                    throw new ApplicationException("User is not found");
                }
                var userInfo = _context.Users.FirstOrDefault(x => x.UserName == _currentUserService.UserName && x.IsActive);

                entity.UserName = dto.UserName;
                entity.LoginName = dto.LoginName;
                entity.UserPassword = dto.UserPassword;
                entity.Code = dto.Code;
                entity.Email = dto.Email;
                entity.MobileNo = dto.MobileNo;
                entity.GenderID = dto.GenderID;
                entity.DateOfBirth = dto.DateOfBirth;
                entity.FromDate = dto.FromDate;
                entity.ToDate = dto.ToDate;
                entity.IsActive = dto.IsActive;
                entity.IsAgent = dto.IsAgent;
                entity.ISD = dto.ISD;
                entity.IsAdmin = dto.IsAdmin;
                //entity.Updated = DateTime.Now;
               // entity.UpdatedBy = userInfo.UserID.ToString();
                _context.Users.Update(entity);


                var userRoles = _context.UserRoles.Where(x => x.UserID == dto.UserID);
                _context.UserRoles.RemoveRange(userRoles);

                var newRoles = request.UserVM.userRolesDto;
                foreach (var role in newRoles)
                {
                    var roleEntity = new UserRoles
                    {
                        UserID = entity.UserID,
                        RoleID = role.RoleID,
                        //Created = DateTime.Now,
                        //CreatedBy = userInfo.UserID.ToString()
                    };
                    await _context.UserRoles.AddAsync(roleEntity, cancellationToken);
                }

                await _context.SaveChangesAsync(cancellationToken);
                return entity.UserID;
            }

        }
    }
}
