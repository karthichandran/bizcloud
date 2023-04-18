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
using ReProServices.Application.Roles;
using ReProServices.Domain;
using ReProServices.Domain.Entities;

namespace ReProServices.Application.User.Queries
{
   public class GetUserByIdQuery : IRequest<UserVM>
    {
        public int UserID { get; set; }

        public class GetUserByIdQueryHandler : IRequestHandler<GetUserByIdQuery, UserVM>
        {
            private readonly IApplicationDbContext _context;
            private readonly IMapper _mapper;
            public GetUserByIdQueryHandler(IApplicationDbContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }
            public async Task<UserVM> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
            {
                var dto =await _context.Users.Where(x => x.UserID == request.UserID)
                    .ProjectTo<UserDto>(_mapper.ConfigurationProvider)
                        .FirstOrDefaultAsync(cancellationToken);    
                var rolesDto= await _context.UserRoles.Where(x => x.UserID == dto.UserID)                  
                        .ToListAsync(cancellationToken);
                List<UserRolesDto> userRoles=new List<UserRolesDto>();

                foreach (var obj in rolesDto) {
                    userRoles.Add(new UserRolesDto
                    {
                        UserID=obj.UserID,
                        RoleID=obj.RoleID
                    });
                }

                var vm = new UserVM
                {
                    userDto = dto,
                    userRolesDto = userRoles
                };
                return vm;
            }
        }
        }
}
