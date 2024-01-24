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
    public class GetRemittanceRemarkQuery : IRequest<List<RemittanceRemarkDto>>
    {
        public RemarkFilter filter { get; set; }
        public class GetRemittanceRemarkQueryHandler : IRequestHandler<GetRemittanceRemarkQuery, List<RemittanceRemarkDto>>
        {

            private readonly IApplicationDbContext _context;

            public GetRemittanceRemarkQueryHandler(IApplicationDbContext context)
            {
                _context = context;
            }

            public async Task<List<RemittanceRemarkDto>> Handle(GetRemittanceRemarkQuery request, CancellationToken cancellationToken)
            {
             
                try
                {
                    List<RemittanceRemarkDto> list;
                    if(request.filter.IsRemittance==null)
                        list = _context.RemittanceRemark.Select(x=>new RemittanceRemarkDto{ RemarkId=x.RemarkId,Description=x.Description,IsRemittance=x.IsRemittance}).ToList();
                    else
                        list = _context.RemittanceRemark.Where(x=>x.IsRemittance==request.filter.IsRemittance).Select(x => new RemittanceRemarkDto { RemarkId = x.RemarkId, Description = x.Description, IsRemittance = x.IsRemittance }).ToList();


                    return list;
                }
                catch (Exception ex)
                {

                    throw ex;
                }


            }
        }
    }
}
