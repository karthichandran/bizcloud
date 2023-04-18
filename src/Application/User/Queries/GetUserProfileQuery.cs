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
    public class GetUserProfileQuery : IRequest<UserProfileDto>
    {
        public string LoginName { get; set; }

        public class GetUserProfileQueryHandler : IRequestHandler<GetUserProfileQuery, UserProfileDto>
        {
            private readonly IApplicationDbContext _context;
            private readonly IMapper _mapper;
            public GetUserProfileQueryHandler(IApplicationDbContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }
            public async Task<UserProfileDto> Handle(GetUserProfileQuery request, CancellationToken cancellationToken)
            {
                var entity = await _context.Users.Where(x => x.LoginName.ToLower() == request.LoginName.ToLower())                    
                        .FirstOrDefaultAsync(cancellationToken);
                var dto = new UserProfileDto
                {
                    UserName=entity.UserName,
                    LoginName=entity.LoginName,
                    Code=entity.Code,
                    Email=entity.Email,
                    MobileNo=entity.MobileNo
                };
                return dto;
            }
        }
    }
}
