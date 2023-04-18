using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using ReProServices.Application.Common.Interfaces;
using ReProServices.Domain;
using ReProServices.Domain.Entities;

namespace ReProServices.Application.User.Command
{
    public class CreateUserSessionCommand : IRequest<int>
    {
        public UserSessionDto UserSessionDto { get; set; }
        public class CreateUserSessionCommandHandler : IRequestHandler<CreateUserSessionCommand, int>
        {
            private readonly IApplicationDbContext _context;
            public CreateUserSessionCommandHandler(IApplicationDbContext context)
            {
                _context = context;
            }
            public async Task<int> Handle(CreateUserSessionCommand request, CancellationToken cancellationToken)
            {
                var sessionDto = request.UserSessionDto;
                UserSession entity = new UserSession
                {
                    UserID = sessionDto.UserID,
                    RefreshToken = sessionDto.RefreshToken,
                    Expires = sessionDto.Expires,
                    Created=sessionDto.Created,
                    CreatedByIp=sessionDto.CreatedByIp,
                };
                await _context.UserSession.AddAsync(entity, cancellationToken);
                await _context.SaveChangesAsync(cancellationToken);               

                return entity.UserID;
            }

        }
    }
}
