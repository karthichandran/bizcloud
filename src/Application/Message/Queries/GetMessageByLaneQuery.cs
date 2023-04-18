using MediatR;
using ReProServices.Application.Common.Interfaces;
using System.Threading.Tasks;
using System.Threading;
using ReProServices.Domain.Entities;
using System.Linq;
using System;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;
namespace ReProServices.Application.Message.Queries
{
    public class GetMessageByLaneQuery : IRequest<MessageDto>
    {
        public int laneNo { get; set; }
        public class GetMessageByLaneQueryHandler : IRequestHandler<GetMessageByLaneQuery, MessageDto>
        {
            private readonly IApplicationDbContext _context;
            private readonly ICurrentUserService _currentUserService;
            public GetMessageByLaneQueryHandler(IApplicationDbContext context, ICurrentUserService currentUserService)
            {
                _context = context;
                _currentUserService = currentUserService;
            }

            public async Task<MessageDto> Handle(GetMessageByLaneQuery request, CancellationToken cancellationToken)
            {
                var qry = "exec sp_GetMessage " + request.laneNo;

                var vm = _context.Message.FromSqlRaw(qry).ToList().FirstOrDefault();

                if (vm == null)
                    return null;

                var model = new MessageDto {MessageID=vm.MessageID,
                    Lane=vm.Lane,
                    Subject=vm.Subject,
                    Body=vm.Body,
                    Opt=vm.Otp,
                    Verified=vm.Verified,
                    Error_code = 0 };

                return model;
            }
        }
    }
}
