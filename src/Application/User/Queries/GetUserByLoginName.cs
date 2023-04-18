using System;
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
    public class GetUserByLoginNameQuery : IRequest<UserDto>
    {
        public string LoginName { get; set; }

        public class GetUserByLoginNameQueryHandler : IRequestHandler<GetUserByLoginNameQuery, UserDto>
        {
            private readonly IApplicationDbContext _context;
            private readonly IMapper _mapper;
            public GetUserByLoginNameQueryHandler(IApplicationDbContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }
            public async Task<UserDto> Handle(GetUserByLoginNameQuery request, CancellationToken cancellationToken)
            {
                var dto = await _context.Users.Where(x => x.LoginName.ToLower() == request.LoginName.ToLower())
                    .ProjectTo<UserDto>(_mapper.ConfigurationProvider)
                        .FirstOrDefaultAsync(cancellationToken);
                return dto;
            }
        }
    }
}
