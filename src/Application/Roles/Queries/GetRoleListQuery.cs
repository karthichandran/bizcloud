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

namespace ReProServices.Application.Roles.Queries
{
    public class GetRoleListQuery : IRequest<List<RolesDto>>
    {
        public class GetRoleListQueryHandler : IRequestHandler<GetRoleListQuery, List<RolesDto>>
        {
            private readonly IApplicationDbContext _context;
            private readonly IMapper _mapper;
            public GetRoleListQueryHandler(IApplicationDbContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }
            public async Task<List<RolesDto>> Handle(GetRoleListQuery request, CancellationToken cancellationToken)
            {
                var dto = await _context.Roles
                    .ProjectTo<RolesDto>(_mapper.ConfigurationProvider)
                        .ToListAsync(cancellationToken);
                return dto;
            }
        }
    }
}
