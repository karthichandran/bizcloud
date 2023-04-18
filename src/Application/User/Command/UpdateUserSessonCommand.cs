using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using ReProServices.Application.Common.Interfaces;
using ReProServices.Domain;
using ReProServices.Domain.Entities;
namespace ReProServices.Application.User.Command
{
   public class UpdateUserSessonCommand : IRequest<int>
    {
        public string Token { get; set; }
        public UserSessionDto UserSessionDto { get; set; }
        public class UpdateUserSessonCommandHandler : IRequestHandler<UpdateUserSessonCommand, int>
        {
            private readonly IApplicationDbContext _context;
            public UpdateUserSessonCommandHandler(IApplicationDbContext context)
            {
                _context = context;
            }
            public async Task<int> Handle(UpdateUserSessonCommand request, CancellationToken cancellationToken)
            {
                var sessionDto = request.UserSessionDto;
                var entity = _context.UserSession.FirstOrDefault(x => x.RefreshToken == request.Token);
                
                entity.RefreshToken = sessionDto.RefreshToken;
                entity.Expires = sessionDto.Expires;
               
                 _context.UserSession.Update(entity);
                await _context.SaveChangesAsync(cancellationToken);

                return entity.UserID;
            }

        }
    }
}
