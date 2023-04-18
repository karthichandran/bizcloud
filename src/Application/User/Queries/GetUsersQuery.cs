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

namespace ReProServices.Application.User.Queries
{
    public class GetUsersQuery: IRequest<List<UserDto>>
    {
        public UserFilter Filter { get; set; } = new UserFilter();
        public class GetUsersQueryHandler : IRequestHandler<GetUsersQuery, List<UserDto>>
        {
            private readonly IApplicationDbContext _context;
            private readonly IMapper _mapper;
            public GetUsersQueryHandler(IApplicationDbContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }
            public async Task<List<UserDto>> Handle(GetUsersQuery request, CancellationToken cancellationToken)
            {
                var dto = await _context.Users
                    .ProjectTo<UserDto>(_mapper.ConfigurationProvider)
                    .PostFilterUsersBy(request.Filter)
                        .ToListAsync(cancellationToken);
                return dto;
            }
        }
    }
}
