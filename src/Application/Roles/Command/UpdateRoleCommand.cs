using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using ReProServices.Application.Common.Interfaces;

namespace ReProServices.Application.Roles.Command
{
    public class UpdateRoleCommand : IRequest<int>
    {
        public RolesDto RolesDto { get; set; }

        public class UpdateRoleCommandHandler : IRequestHandler<UpdateRoleCommand, int>
        {
            private readonly IApplicationDbContext _context;
            private readonly ICurrentUserService _currentUserService;
            public UpdateRoleCommandHandler(IApplicationDbContext context, ICurrentUserService currentUserService)
            {
                _context = context;
                _currentUserService = currentUserService;
            }

            public async Task<int> Handle(UpdateRoleCommand request, CancellationToken cancellationToken)
            {
                var userInfo = _context.Users.FirstOrDefault(x => x.UserName == _currentUserService.UserName && x.IsActive);
                var dto = request.RolesDto;
                var entity = _context.Roles.FirstOrDefault(x => x.RoleID == dto.RoleID);

                entity.Name = dto.Name;
                entity.ReportingTo = dto.ReportingTo;
                entity.IsOrganizationRole = dto.IsOrganizationRole;
                entity.Updated = DateTime.Now;
                entity.UpdatedBy = userInfo.UserID.ToString();

                _context.Roles.Update(entity);
                await _context.SaveChangesAsync(cancellationToken);
                return entity.RoleID;
            }

        }
    }
}
