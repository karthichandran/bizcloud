using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using ReProServices.Application.Common.Interfaces;
using ReProServices.Domain;
using ReProServices.Domain.Entities;

namespace ReProServices.Application.Segment.Command
{
    public class CreateRolePermissionsCommand : IRequest<int>
    {
        public List<SegmentPermissionsDto> permissionsDto { get; set; }

        public class CreateRolePermissionsCommandHandler : IRequestHandler<CreateRolePermissionsCommand, int>
        {
            private readonly IApplicationDbContext _context;
            private readonly ICurrentUserService _currentUserService;
            public CreateRolePermissionsCommandHandler(IApplicationDbContext context, ICurrentUserService currentUserService)
            {
                _context = context;
                _currentUserService = currentUserService;
            }

            public async Task<int> Handle(CreateRolePermissionsCommand request, CancellationToken cancellationToken)
            {
                var dto = request.permissionsDto;
                var userInfo = _context.Users.FirstOrDefault(x => x.UserName == _currentUserService.UserName && x.IsActive);
                foreach (var obj in dto) {
                   var  entity = new SegmentRolePermissions
                    {
                       SegmentID=obj.SegmentID,
                       RoleID=obj.RoleID,
                       CreatePerm=obj.CreatePerm,
                       EditPerm=obj.EditPerm,
                       ViewPerm=obj.ViewPerm,
                       DeletePerm=obj.DeletePerm,
                       //Created = DateTime.Now,
                       //CreatedBy = userInfo.UserID.ToString()
                   };
                    await _context.SegmentRolePermissions.AddAsync(entity, cancellationToken);
                }
                               
                await _context.SaveChangesAsync(cancellationToken);
                return 0;
            }

        }
    }
}
