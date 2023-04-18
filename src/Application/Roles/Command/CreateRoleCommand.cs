using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ReProServices.Application.Common.Interfaces;
using ReProServices.Domain;
using ReProServices.Domain.Entities;

namespace ReProServices.Application.Roles.Command
{
    public class CreateRoleCommand : IRequest<int>
    {
        public RolesDto RolesDto { get; set; }

        public class CreateRoleCommandHandler : IRequestHandler<CreateRoleCommand, int> {
            private readonly IApplicationDbContext _context;
            private readonly ICurrentUserService _currentUserService;
            public CreateRoleCommandHandler(IApplicationDbContext context, ICurrentUserService currentUserService) {
                _context = context;
                _currentUserService = currentUserService;
            }

            public async Task<int> Handle(CreateRoleCommand request, CancellationToken cancellationToken)
            {
                var userInfo = _context.Users.FirstOrDefault(x => x.UserName == _currentUserService.UserName && x.IsActive);
                var dto = request.RolesDto;
                Domain.Entities.Roles entity = new Domain.Entities.Roles
                {
                   Name=dto.Name,
                   ReportingTo=dto.ReportingTo,
                   IsOrganizationRole=dto.IsOrganizationRole,
                   Created = DateTime.Now,
                   CreatedBy = userInfo.UserID.ToString()
                };

                await _context.Roles.AddAsync(entity, cancellationToken);
                await _context.SaveChangesAsync(cancellationToken);
                return entity.RoleID;
            }

        }
    }
}
