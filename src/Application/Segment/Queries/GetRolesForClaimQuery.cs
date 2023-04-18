using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ReProServices.Application.Common.Interfaces;
using ReProServices.Domain;
using ReProServices.Domain.Entities;

namespace ReProServices.Application.Segment.Queries
{
    public class GetRolesForClaimQuery : IRequest<List<string>>
    {
        public int UserID { get; set; }
        public class GetRolesForClaimQueryHandler : IRequestHandler<GetRolesForClaimQuery, List<string>>
        {
            private readonly IApplicationDbContext _context;
            private readonly IMapper _mapper;
            public GetRolesForClaimQueryHandler(IApplicationDbContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }
            public async Task<List<string>> Handle(GetRolesForClaimQuery request, CancellationToken cancellationToken)
            {
                var userInfo = _context.Users.Where(x => x.UserID == request.UserID).FirstOrDefault();
                var userRoles = _context.UserRoles.Where(x => x.UserID == request.UserID);
                List<string> roles = new List<string>();
                foreach (var role in userRoles) {
                    var segment = from perm in _context.SegmentRolePermissions
                                  join seg in _context.Segment on perm.SegmentID equals seg.SegmentID
                                  where (perm.RoleID == role.RoleID)
                                  select new { perm, seg.Name };

                    if (userInfo.IsAdmin) {
                        roles.Add("Admin");
                    }

                    foreach (var seg in segment) {
                        if (seg.perm.CreatePerm) { 
                        if(roles.IndexOf(seg.Name+"_Create") == -1)
                                roles.Add(seg.Name + "_Create");
                        }
                        if (seg.perm.EditPerm)
                        {
                            if (roles.IndexOf(seg.Name +"_Edit") == -1)
                                roles.Add(seg.Name + "_Edit");
                        }
                        if (seg.perm.ViewPerm)
                        {
                            if (roles.IndexOf(seg.Name + "_View") == -1)
                                roles.Add(seg.Name + "_View");
                        }
                        if (seg.perm.DeletePerm)
                        {
                            if (roles.IndexOf(seg.Name + "_Delete") == -1)
                                roles.Add(seg.Name + "_Delete");
                        }
                    }
                }
                
                return roles;
            }
        }
    }
}
