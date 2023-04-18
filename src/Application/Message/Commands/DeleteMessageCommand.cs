using MediatR;
using ReProServices.Application.Common.Interfaces;
using System.Threading.Tasks;
using System.Threading;
using ReProServices.Domain.Entities;
using System.Linq;
using System;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;

namespace ReProServices.Application.Message.Commands
{
   public class DeleteMessageCommand : IRequest<bool>
    {
       public int laneNo { get; set; }
        public class DeleteMessageCommandHandler : IRequestHandler<DeleteMessageCommand, bool>
        {
            private readonly IApplicationDbContext _context;
            private readonly ICurrentUserService _currentUserService;
            public DeleteMessageCommandHandler(IApplicationDbContext context, ICurrentUserService currentUserService)
            {
                _context = context;
                _currentUserService = currentUserService;
            }

            public async Task<bool> Handle(DeleteMessageCommand request, CancellationToken cancellationToken)
            {
                try
                {
                    var model = _context.Message.Where(x => x.Lane == request.laneNo).ToList();
                    _context.Message.RemoveRange(model);
                    await _context.SaveChangesAsync(cancellationToken);
                    return true;
                }
                catch (Exception ex) {
                    return false;
                }
            }
        }
    }
}
