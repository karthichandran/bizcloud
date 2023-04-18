using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ReProServices.Application.Common.Interfaces;

namespace ReProServices.Application.StatusReport.Queries
{
    public class GetStatusReportQuery : IRequest<IList<StatusReportDto>>
    {
        public StatusReportFilter Filter { get; set; } = new StatusReportFilter();

        public class GetStatusReportQueryHandler : IRequestHandler<GetStatusReportQuery, IList<StatusReportDto>> {
            private readonly IApplicationDbContext _context;
            public GetStatusReportQueryHandler(IApplicationDbContext context) {
                _context = context;
            }

            public async Task<IList<StatusReportDto>> Handle(GetStatusReportQuery request, CancellationToken cancellationToken) {

                var vm = (from rt in _context.ViewRemittance
                          join cp in _context.ViewCustomerPropertyBasic on new { rt.OwnershipID, rt.CustomerID } equals new { cp.OwnershipID, cp.CustomerID }
                          join pay in _context.ClientPayment on rt.ClientPaymentID equals pay.ClientPaymentID
                          select new StatusReportDto
                          {
                              PropertyID = cp.PropertyID,
                              CustomerName = cp.CustomerName,
                              Premises = cp.PropertyPremises,
                              UnitNo = cp.UnitNo,
                              LotNo = pay.LotNo,
                              Form16BRequested = rt.F16BDateOfReq,
                              Form16BDownloaded = rt.Form16BFileDownloadDate,
                              MailDate = rt.EmailSentDate,
                              RemittanceOfTdsAmount=rt.F16CreditedAmount,
                              PaymentReceiptDate = rt.F16UpdateDate,
                          }).PreFilterReportBy(request.Filter)
                        .ToList()
                        .AsQueryable()
                        .PostFilterReportBy(request.Filter)
                        .ToList();
                return vm;
            }
        }
    }
}
