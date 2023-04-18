using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ReProServices.Application.Common.Interfaces;
using ReProServices.Application.CustomerBillings;
using ReProServices.Application.CustomerBillings.Queries;
using ReProServices.Domain.Enums;

namespace ReProServices.Application.StatementOfAccount.Queries
{
    public class GetStatementOfAccountReport : IRequest<List<Domain.Entities.StatementOfAccount>>
    {
        public Guid OwnershipId { get; set; }
        public StatementOfAccountFilter StatementOfAccountFilter { get; set; }
        public class GetStatementOfAccountReportHandler :
                              IRequestHandler<GetStatementOfAccountReport, List<Domain.Entities.StatementOfAccount>>
        {

            private readonly IApplicationDbContext _context;
    
            public GetStatementOfAccountReportHandler(IApplicationDbContext context)
            {
                _context = context;
            }

        

            public async Task<List<Domain.Entities.StatementOfAccount>> Handle(GetStatementOfAccountReport request, CancellationToken cancellationToken)
            {
                var filter = request.StatementOfAccountFilter;
                var FilteredOwnershidIds =( from cs in _context.Customer
                                          join cp in _context.CustomerProperty on cs.CustomerID equals cp.CustomerId
                                          where (filter.CustomerName != null) ?  cs.Name.Contains(filter.CustomerName):true && (filter.PropertyId!=null)? cp.PropertyId == filter.PropertyId :true && (filter.UnitNo != null) ? cp.UnitNo == filter.UnitNo:true
                                          select cp.OwnershipID).Distinct().ToList() ;
                var vm = new List<Domain.Entities.StatementOfAccount>();

                var payableClientPayments = GetPayableClientPayments(FilteredOwnershidIds);
                if (payableClientPayments != null && payableClientPayments.Any())
                    vm.AddRange(payableClientPayments);

                var payableServiceCharge = GetPayableCustomerBills(FilteredOwnershidIds);
                if(payableServiceCharge != null && payableServiceCharge.Any())
                vm.AddRange(payableServiceCharge);

                var receivedPayments = GetReceivedPayments(FilteredOwnershidIds);
                if (receivedPayments != null && receivedPayments.Any())
                    vm.AddRange(receivedPayments);

                var receivedServiceCharge = GetReceivedServiceFee(FilteredOwnershidIds);
                if (receivedServiceCharge != null && receivedServiceCharge.Any())
                    vm.AddRange(receivedServiceCharge);

                var tdsRemitteance = GetPaidRemittance(FilteredOwnershidIds);
                if (tdsRemitteance != null && tdsRemitteance.Any())
                    vm.AddRange(tdsRemitteance);



                return vm;
            }

            private IEnumerable<Domain.Entities.StatementOfAccount> GetPayableClientPayments(List<Guid?> requestOwnershipId)
            {
                var payments = from cp in _context.ViewPayableClientPayments
                    where requestOwnershipId.Contains( cp.OwnershipID)
                    select new Domain.Entities.StatementOfAccount
                    {
                        OwnershipID = cp.OwnershipID,
                        PayableAmountPaid = cp.PayableAmountPaid,
                        PayableDateOfPayment = cp.PayableDateOfPayment,
                        PayableGrossAmount = cp.PayableGrossAmount,
                        PayableGst = cp.PayableGst,
                        PayableLateFee = cp.PayableLateFee,
                        PayableReceiptNo = cp.PayableReceiptNo,
                        PayableTds = cp.PayableTds,
                        UnitNo = cp.UnitNo,
                        PayableInterest=cp.PayableInterest
                    };
                return payments;
            } 
            
            private IEnumerable<Domain.Entities.StatementOfAccount> GetPayableCustomerBills(List<Guid?> requestOwnershipId)
            {
                var payments = from cp in _context.CustomerBilling
                               join ct in _context.CustomerProperty on cp.OwnershipID equals ct.OwnershipID
                               where requestOwnershipId.Contains(cp.OwnershipID)
                               && ct.IsPrimaryOwner == true
                               select new Domain.Entities.StatementOfAccount
                    {
                        OwnershipID = cp.OwnershipID,
                        PayableServiceFee = cp.TotalPayable,
                        PayableDateOfPayment = cp.BillDate,
                        UnitNo=ct.UnitNo
                    };
                return payments;
            }

            public IEnumerable<Domain.Entities.StatementOfAccount> GetReceivedPayments(List<Guid?> requestOwnershipId)
            {

                var vm = (from rec in _context.ViewReceipt
                    where //rec.OwnershipID == requestOwnershipId &&
                       requestOwnershipId.Contains(rec.OwnershipID) &&
                   rec.ReceiptType == (int)EReceiptType.Tds
                          select new Domain.Entities.StatementOfAccount
                    {
                        OwnershipID = rec.OwnershipID,
                        ReceivedDate = rec.DateOfReceipt,
                        ReceivedInterest = rec.TdsInterestReceived,
                        ReceivedLateFee = rec.LateFeeReceived,
                        ReceivedTds = rec.TdsReceived,
                        ReceivedTotalAmount = (rec.TdsInterestReceived + rec.LateFeeReceived + rec.TdsReceived),
                        UnitNo=rec.UnitNo
                    });
                   
                return vm;

            }

            public IEnumerable<Domain.Entities.StatementOfAccount> GetReceivedServiceFee(List<Guid?> requestOwnershipId)
            {

                var vm = (from rec in _context.ViewReceipt
                    where //rec.OwnershipID == requestOwnershipId &&
                     requestOwnershipId.Contains(rec.OwnershipID) &&
                    rec.ReceiptType == (int)EReceiptType.ServiceFee
                    select new Domain.Entities.StatementOfAccount
                    {
                        OwnershipID = rec.OwnershipID,
                        ReceivedDate = rec.DateOfReceipt,
                        ReceivedInterest = rec.TdsInterestReceived,
                        ReceivedLateFee = rec.LateFeeReceived,
                        ReceivedTds = rec.TdsReceived,
                        ReceivedTotalAmount = (rec.TdsInterestReceived + rec.LateFeeReceived + rec.TdsReceived),
                        UnitNo = rec.UnitNo,
                        ReceivedServiceCharge=rec.TotalServiceFeeReceived
                    });

                return vm;

            }

            public IEnumerable<Domain.Entities.StatementOfAccount> GetPaidRemittance(List<Guid?> requestOwnershipId)
            {

                var remittances = (from pay in _context.ClientPayment
                    join cpt in _context.ClientPaymentTransaction on pay.ClientPaymentID equals cpt.ClientPaymentID
                    join cp in _context.ViewCustomerPropertyExpanded on new {cpt.OwnershipID, cpt.CustomerID} equals new
                        {cp.OwnershipID, cp.CustomerID}
                    join sp in _context.ViewSellerPropertyExpanded on cp.PropertyID equals sp.PropertyID
                    join r in _context.Remittance on cpt.ClientPaymentTransactionID equals r.ClientPaymentTransactionID
                    join remSt in _context.RemittanceStatus on cpt.RemittanceStatusID equals remSt.RemittanceStatusID
                    where cpt.RemittanceStatusID >= (int) ERemittanceStatus.TdsPaid
                         // && cp.OwnershipID == requestOwnershipId
                         && requestOwnershipId.Contains(cp.OwnershipID)
                          && cpt.SellerID == sp.SellerID
                    select new Domain.Entities.StatementOfAccount
                    {
                        RemittedDate =  pay.DateOfDeduction,
                        RemittedInterest = cpt.TdsInterest,
                        RemittedLateFee = cpt.LateFee,
                        RemittedTds = cpt.Tds,
                        OwnershipID = pay.OwnershipID,
                        PayableReceiptNo = pay.ReceiptNo,
                        UnitNo=cp.UnitNo
                    });
                return remittances;
            }





        }
    }
}
