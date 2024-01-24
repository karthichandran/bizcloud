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
    public class UpdateRemittanceRemarkCommand : IRequest<bool>
    {
        public RemittanceRemarkDto remittanceRemarkDto { get; set; }
        public class UpdateRemittanceRemarkCommandHandler : IRequestHandler<UpdateRemittanceRemarkCommand, bool>
        {

            private readonly IApplicationDbContext _context;

            public UpdateRemittanceRemarkCommandHandler(IApplicationDbContext context)
            {
                _context = context;
            }

            public async Task<bool> Handle(UpdateRemittanceRemarkCommand request, CancellationToken cancellationToken)
            {
                var model = request.remittanceRemarkDto;
                try
                {

                    var entity = _context.RemittanceRemark.Where(x => x.RemarkId == model.RemarkId).FirstOrDefault();
                    if (entity == null)
                        return false;
                    entity.Description = model.Description;
                    entity.IsRemittance = model.IsRemittance;

                    _context.RemittanceRemark.Update(entity);
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
