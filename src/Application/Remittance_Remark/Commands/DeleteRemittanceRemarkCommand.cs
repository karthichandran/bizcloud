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
   public class DeleteRemittanceRemarkCommand : IRequest<bool>
    {
        public int remarkId;
        public class DeleteRemittanceRemarkCommandHandler : IRequestHandler<DeleteRemittanceRemarkCommand, bool>
        {


            private readonly IApplicationDbContext _context;

            public DeleteRemittanceRemarkCommandHandler(IApplicationDbContext context)
            {
                _context = context;
            }
            public async Task<bool> Handle(DeleteRemittanceRemarkCommand request, CancellationToken cancellationToken)
            {
                try
                {
                    var entity = _context.RemittanceRemark.Where(x => x.RemarkId == request.remarkId).FirstOrDefault();
                    if (entity == null)
                        return false;

                    _context.RemittanceRemark.Remove(entity);
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
