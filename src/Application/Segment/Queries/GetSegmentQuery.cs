using System;
using System.Collections.Generic;
using System.Linq;
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
   public class GetSegmentQuery:IRequest<List<SegmentPermissionsDto>>
    {
        public class GetSegmentQueryHandler : IRequestHandler<GetSegmentQuery, List<SegmentPermissionsDto>>
        {
            private readonly IApplicationDbContext _context;
            private readonly IMapper _mapper;
            public GetSegmentQueryHandler(IApplicationDbContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }
            public async Task<List<SegmentPermissionsDto>> Handle(GetSegmentQuery request, CancellationToken cancellationToken)
            {
                var dto = await _context.Segment.ToListAsync(cancellationToken);

                List<SegmentPermissionsDto> segmentList = new List<SegmentPermissionsDto>();
                foreach (var obj in dto) {
                    segmentList.Add(new SegmentPermissionsDto {
                    SegmentID=obj.SegmentID,
                    Name=obj.Name
                    });
                }
                return segmentList;
            }
        }
    }
}
