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
    public class CreateMessageCommand : IRequest<MessageDto>
    {
        public string subject { get; set; }
        public string  message { get; set; }
        public int laneNo { get; set; }

        public class CreateMessageCommandHandler : IRequestHandler<CreateMessageCommand, MessageDto>
        {
            private readonly IApplicationDbContext _context;
            private readonly ICurrentUserService _currentUserService;
            public CreateMessageCommandHandler(IApplicationDbContext context, ICurrentUserService currentUserService)
            {
                _context = context;
                _currentUserService = currentUserService;
            }

            public async Task<MessageDto> Handle(CreateMessageCommand request, CancellationToken cancellationToken)
            {

                var incomeMsg = Regex.Split(request.message, "Message:");             

                var Msg = incomeMsg.Length > 1 ? incomeMsg[1] : request.message;

                if (!Msg.Contains(" OTP ")) {
                    return new MessageDto { Error_code = 0 };
                }

                string match = Regex.Match(Msg, "(\\d{6})").Groups[0].Value;
              
                var entity = new Domain.Entities.Message
                {
                    MessageID = 0,
                    Subject = request.subject,
                    Body = request.message,
                    Verified = false,
                    Lane=request.laneNo,
                    Otp=string.IsNullOrEmpty(match)?0: Convert.ToInt32(match)

                };
                await _context.Message.AddAsync(entity, cancellationToken);
                await _context.SaveChangesAsync(cancellationToken);

               return new MessageDto { Error_code = 0 };       
               
               
            }
        }
    }
}
