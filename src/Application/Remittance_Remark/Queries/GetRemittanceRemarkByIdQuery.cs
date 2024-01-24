using MediatR;
using ReProServices.Application.Common.Interfaces;
using System.Threading.Tasks;
using System.Threading;
using ReProServices.Domain.Entities;
using System.Linq;
using System;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;
using System.Collections.Generic;

namespace ReProServices.Application.Remittance_Remark.Queries
{
   public class GetRemittanceRemarkByIdQuery : IRequest<RemittanceRemarkDto>    {
        public int Id { get; set; }
        public class GetRemittanceRemarkByIdQueryHandler : IRequestHandler<GetRemittanceRemarkByIdQuery, RemittanceRemarkDto>
        {

            private readonly IApplicationDbContext _context;

            public GetRemittanceRemarkByIdQueryHandler(IApplicationDbContext context)
            {
                _context = context;
            }

            public async Task<RemittanceRemarkDto> Handle(GetRemittanceRemarkByIdQuery request, CancellationToken cancellationToken)
            {

                try
                {                 
                        var model = _context.RemittanceRemark.Where(x => x.RemarkId == request.Id).Select(x => new RemittanceRemarkDto { RemarkId = x.RemarkId, Description = x.Description, IsRemittance = x.IsRemittance }).FirstOrDefault();

                    return model;
                }
                catch (Exception ex)
                {

                    throw ex;
                }
            }
        }
    }
}
