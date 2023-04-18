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
    public class UpdateRolePermissionsCommand : IRequest<int>
    {
        public List<SegmentPermissionsDto> permissionsDto { get; set; }

        public class UpdateRolePermissionsCommandHandler : IRequestHandler<UpdateRolePermissionsCommand, int>
        {
            private readonly IApplicationDbContext _context;
            private readonly ICurrentUserService _currentUserService;
            public UpdateRolePermissionsCommandHandler(IApplicationDbContext context, ICurrentUserService currentUserService)
            {
                _context = context;
                _currentUserService = currentUserService;
            }

            public async Task<int> Handle(UpdateRolePermissionsCommand request, CancellationToken cancellationToken)
            {
                var dto = request.permissionsDto;
                var userInfo = _context.Users.FirstOrDefault(x => x.UserName == _currentUserService.UserName && x.IsActive);
                foreach (var obj in dto)
                {
                    var existEntity = _context.SegmentRolePermissions.Where(x => x.SegmentRolePermissionsID==obj.SegmentRolePermissionsID).FirstOrDefault();
                    if (existEntity == null) {
                        var entity = new SegmentRolePermissions
                        {
                            SegmentID = obj.SegmentID,
                            RoleID = obj.RoleID,
                            CreatePerm = obj.CreatePerm,
                            EditPerm = obj.EditPerm,
                            ViewPerm = obj.ViewPerm,
                            DeletePerm = obj.DeletePerm,
                            //Created = DateTime.Now,
                            //CreatedBy= userInfo.UserID.ToString()
                    };
                        await _context.SegmentRolePermissions.AddAsync(entity, cancellationToken);
                    }
                    else
                    {
                        existEntity.CreatePerm = obj.CreatePerm;
                        existEntity.EditPerm = obj.EditPerm;
                        existEntity.ViewPerm = obj.ViewPerm;
                        existEntity.DeletePerm = obj.DeletePerm;
                        //existEntity.Updated = DateTime.Now;
                        //existEntity.UpdatedBy = userInfo.UserID.ToString();
                        _context.SegmentRolePermissions.Update(existEntity);
                    }                   
                }

                await _context.SaveChangesAsync(cancellationToken);
                return 0;
            }

        }
    }
}
