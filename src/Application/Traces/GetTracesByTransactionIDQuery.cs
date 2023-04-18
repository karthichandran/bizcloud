using System;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ReProServices.Application.Common.Interfaces;
using ReProServices.Application.TdsRemittance;
using NodaTime;

namespace ReProServices.Application.Traces
{
    public class GetTracesByTransactionIDQuery : IRequest<TdsRemittanceDto>
    {
        public int ClientPaymentTransactionID { get; set; }
        public class GetTracesListByTransactionIDQueryHandler : IRequestHandler<GetTracesByTransactionIDQuery, TdsRemittanceDto>
        {

            private readonly IApplicationDbContext _context;

            public GetTracesListByTransactionIDQueryHandler(IApplicationDbContext context)
            {
                _context = context;
            }

            public async Task<TdsRemittanceDto> Handle(GetTracesByTransactionIDQuery request, CancellationToken cancellationToken)
            {

                var remittance = await (from pay in _context.ClientPayment
                        join cpt in _context.ClientPaymentTransaction on pay.ClientPaymentID equals cpt.ClientPaymentID
                        join cp in _context.ViewCustomerPropertyExpanded on new {cpt.OwnershipID, cpt.CustomerID} equals
                            new {cp.OwnershipID, cp.CustomerID}
                        join sp in _context.ViewSellerPropertyBasic on cp.PropertyID equals sp.PropertyID
                        join rem in _context.Remittance on cpt.ClientPaymentTransactionID equals rem
                            .ClientPaymentTransactionID
                        where cpt.ClientPaymentTransactionID == request.ClientPaymentTransactionID
                              && cpt.SellerID == sp.SellerID
                        select new TdsRemittanceDto
                        {
                            ClientPaymentTransactionID = cpt.ClientPaymentTransactionID,
                            CustomerName = cp.CustomerName,
                            CustomerShare = cpt.CustomerShare,
                            SellerName = sp.SellerName,
                            SellerShare = cpt.SellerShare,
                            PropertyPremises = sp.PropertyPremises,
                            UnitNo = cp.UnitNo,
                            OwnershipID = cp.OwnershipID,
                            InstallmentID = pay.InstallmentID,
                            StatusTypeID = cp.StatusTypeID,
                            GstAmount = cpt.Gst,
                            TdsInterest = cpt.TdsInterest,
                            AmountPaid = pay.AmountPaid,
                            GrossAmount = pay.GrossAmount,
                            RevisedDateOfPayment = pay.RevisedDateOfPayment,
                            DateOfDeduction = pay.DateOfDeduction,
                            ReceiptNo = pay.ReceiptNo,
                            LateFee = cpt.LateFee,
                            ClientPaymentID = pay.ClientPaymentID,
                            LotNo = pay.LotNo,
                            GrossShareAmount = cpt.GrossShareAmount,
                            TdsAmount = cpt.Tds,
                            AmountShare = cpt.ShareAmount,
                            ChallanDate = rem.ChallanDate,
                            ChallanAckNo = rem.ChallanAckNo,
                            CustomerPAN = cp.CustomerPAN,
                            SellerPAN = sp.SellerPAN,
                            TracesPassword = cp.TracesPassword,
                           
                        })
                    .FirstOrDefaultAsync(cancellationToken: cancellationToken);
                remittance.AssessmentYear = GetAssessmentYear(remittance.RevisedDateOfPayment);
                return remittance;
            }

            private string GetAssessmentYear(DateTime revisedDateOfPayment)
            {
                const int FiscalYearEnd = 3; //March
                string assesYear;
                var nodaDate = LocalDate.FromDateTime(revisedDateOfPayment);
                if (nodaDate.Month > FiscalYearEnd)
                {
                    nodaDate = nodaDate.PlusYears(1);
                   
                }

                assesYear = nodaDate.Year.ToString() + "-" + nodaDate.PlusYears(1).Year.ToString().Substring(2);
                return assesYear;
            }
        }
    }
}
