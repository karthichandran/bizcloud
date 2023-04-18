using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using ReProServices.Application.Common.Interfaces;

namespace ReProServices.Application.Prospect.Command
{
    public class UpdateProspectSharesCommand : IRequest<int>
    {
        public ProspectVm prospectVm { get; set; }

        public class UpdateProspectSharesCommandHandler : IRequestHandler<UpdateProspectSharesCommand, int>
        {
            private readonly IApplicationDbContext _context;
            public UpdateProspectSharesCommandHandler(IApplicationDbContext context)
            {
                _context = context;
            }
            public async Task<int> Handle(UpdateProspectSharesCommand request, CancellationToken cancellationToken)
            {
                var dtoList = request.prospectVm.ProspectDto;
                foreach (var dto in dtoList)
                {
                    var entity = _context.Prospect.FirstOrDefault(x => x.ProspectID == dto.ProspectID);

                    if (entity == null)
                    {
                        throw new ApplicationException("User is not found");
                    }
                    
                    entity.Share = dto.Share;
                    _context.Prospect.Update(entity);
                }
                await _context.SaveChangesAsync(cancellationToken);
                return 0;
            }

        }
    }
}
