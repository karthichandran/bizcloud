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
    public class GetUserSessionQuery : IRequest<UserSessionDto>
    {
        public string Token { get; set; }

        public class GetUserSessionQueryHandler : IRequestHandler<GetUserSessionQuery, UserSessionDto>
        {
            private readonly IApplicationDbContext _context;
            private readonly IMapper _mapper;
            public GetUserSessionQueryHandler(IApplicationDbContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }
            public async Task<UserSessionDto> Handle(GetUserSessionQuery request, CancellationToken cancellationToken)
            {
                var dto = await _context.UserSession.Where(x => x.RefreshToken == request.Token)
                    .ProjectTo<UserSessionDto>(_mapper.ConfigurationProvider)
                        .FirstOrDefaultAsync(cancellationToken);
                
                return dto;
            }
        }
    }
}
