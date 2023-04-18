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
   public class GetRoleByIdQuery : IRequest<RolesDto>
    {
        public int RoleID { get; set; }
        public class GetRoleByIdQueryHandler : IRequestHandler<GetRoleByIdQuery, RolesDto>
        {
            private readonly IApplicationDbContext _context;
            private readonly IMapper _mapper;
            public GetRoleByIdQueryHandler(IApplicationDbContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }
            public async Task<RolesDto> Handle(GetRoleByIdQuery request, CancellationToken cancellationToken)
            {
                var dto = await _context.Roles.Where(x => x.RoleID == request.RoleID)
                    .ProjectTo<RolesDto>(_mapper.ConfigurationProvider).FirstOrDefaultAsync(cancellationToken);
                return dto;
            }
        }
    }
}
