using MediatR;
using ReProServices.Application.Common.Interfaces;
using System.Threading.Tasks;
using System.Threading;
using ReProServices.Domain.Entities;
using System.Linq;
using System;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;

namespace ReProServices.Application.Remittance_Remark.Commands
{
    public class CreateRemittanceRemarkCommand: IRequest<bool>
    {
        public RemittanceRemarkDto remittanceRemarkDto { get; set; }
        public class CreateRemittanceRemarkCommandHandler : IRequestHandler<CreateRemittanceRemarkCommand, bool>
        {

            private readonly IApplicationDbContext _context;

            public CreateRemittanceRemarkCommandHandler(IApplicationDbContext context)
            {
                _context = context;
            }

            public async Task<bool> Handle(CreateRemittanceRemarkCommand request, CancellationToken cancellationToken)
            {
                var model = request.remittanceRemarkDto;
                try
                {
                    var entity = new Domain.Entities.RemittanceRemark
                    {
                        Description = model.Description,
                        IsRemittance = model.IsRemittance

                    };
                    await _context.RemittanceRemark.AddAsync(entity, cancellationToken);
                    await _context.SaveChangesAsync(cancellationToken);

                    return true;
                }
                catch (Exception ex)
                {

                    throw ex;
                }


            }
        }
    }
}
