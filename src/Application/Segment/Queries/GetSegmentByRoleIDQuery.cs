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
    public class GetSegmentByRoleIDQuery : IRequest<List<SegmentPermissionsDto>>
    {
        public int RoleID { get; set; }
        public class GetSegmentByRoleIDQueryHandler : IRequestHandler<GetSegmentByRoleIDQuery, List<SegmentPermissionsDto>>
        {
            private readonly IApplicationDbContext _context;
            private readonly IMapper _mapper;
            public GetSegmentByRoleIDQueryHandler(IApplicationDbContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }
            public async Task<List<SegmentPermissionsDto>> Handle(GetSegmentByRoleIDQuery request, CancellationToken cancellationToken)
            {
                List<SegmentPermissionsDto> segmentList = new List<SegmentPermissionsDto>();

                var existSegment = await _context.SegmentRolePermissions
                    .Where(x => x.RoleID == request.RoleID)
                    .ToListAsync(cancellationToken);

                if (existSegment.Count > 0)
                {


                    segmentList = await (from seg in _context.Segment
                                         join per in _context.SegmentRolePermissions.Where(x=>x.RoleID == request.RoleID) on seg.SegmentID equals per.SegmentID into roleList
                                         from pr in roleList.DefaultIfEmpty()
                                         select (new SegmentPermissionsDto
                                         {
                                             SegmentID = seg.SegmentID,
                                             SegmentRolePermissionsID = pr.SegmentRolePermissionsID,
                                             RoleID = pr.RoleID,
                                             Name = seg.Name,
                                             CreatePerm = pr.CreatePerm,
                                             EditPerm = pr.EditPerm,
                                             ViewPerm = pr.ViewPerm,
                                             DeletePerm = pr.DeletePerm
                                         })).ToListAsync<SegmentPermissionsDto>(cancellationToken);


                    //segmentList = await (from seg in _context.Segment 
                    //               join per in _context.SegmentRolePermissions on seg.SegmentID equals per.SegmentID
                    //               where (per.RoleID == request.RoleID)
                    //               select (new SegmentPermissionsDto
                    //               {
                    //                   SegmentID = per.SegmentID,
                    //                   SegmentRolePermissionsID = per.SegmentRolePermissionsID,
                    //                   RoleID = per.RoleID,
                    //                   Name = seg.Name,
                    //                   CreatePerm = per.CreatePerm,
                    //                   EditPerm = per.EditPerm,
                    //                   ViewPerm = per.ViewPerm,
                    //                   DeletePerm = per.DeletePerm
                    //               })).ToListAsync<SegmentPermissionsDto>(cancellationToken);
                }
                else
                {
                    var dto = await _context.Segment.ToListAsync(cancellationToken);
                    foreach (var obj in dto)
                    {
                        segmentList.Add(new SegmentPermissionsDto
                        {
                            SegmentID = obj.SegmentID,
                            Name = obj.Name
                        });
                    }
                }

                return segmentList;
            }
        }
    }
}
