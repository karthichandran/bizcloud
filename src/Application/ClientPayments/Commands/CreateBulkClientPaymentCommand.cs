using MediatR;
using ReProServices.Application.Common.Interfaces;
using System.Threading.Tasks;
using System.Threading;
using System;
using System.Linq;
using ReProServices.Domain.Entities;
using ReProServices.Application.Common.Formulas;
using System.Transactions;
using ReProServices.Domain.Enums;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using ReProServices.Application.TaxCodes;
using NodaTime;

namespace ReProServices.Application.ClientPayments.Commands
{
    public class CreateBulkClientPaymentCommand : IRequest<Unit>
    {
        public List<ClientPaymentDto> ClientPaymentVMs { get; set; }
        public List<TaxCodesDto> TaxCodes { get; set; }
        public class CreateBulkClientPaymentCommandHandler : IRequestHandler<CreateBulkClientPaymentCommand, Unit>
        {
            public List<TaxCodesDto> taxCodes = null;
            private readonly IApplicationDbContext _context;
            public CreateBulkClientPaymentCommandHandler(IApplicationDbContext context)
            {
                _context = context;
            }

            public async Task<Unit> Handle(CreateBulkClientPaymentCommand request, CancellationToken cancellationToken)
            {
                taxCodes = request.TaxCodes;
                //bool isDuplicateReceipt = (from cp in _context.ClientPayment
                //                           join pi in _context.ViewClientPaymentImports on cp.ReceiptNo equals pi.ReceiptNo
                //                           select new ClientPaymentDto {
                //                               ReceiptNo = cp.ReceiptNo
                //                           }).ToList().Count() > 0;

                //if (isDuplicateReceipt)
                //{
                //    throw new ApplicationException("App trying to insert duplicate receipt");
                //}


                //where cp.OwnershipID == request.OwnershipId TODO filter based on LotNo

                var payments = new List<ClientPayment>();

                var vms = request.ClientPaymentVMs;
                using var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
                foreach (var vm in vms)
                {
                    var payObj = vm;
                    payObj.DateOfDeduction = CalculateDeductionDate(payObj.RevisedDateOfPayment.Value);
                    var clientPay = new ClientPayment
                    {
                        AmountPaid = payObj.AmountPaid,
                        Created = DateTime.Now,
                        CreatedBy = "", //TODO Fix once identity service is updated
                        DateOfDeduction = payObj.DateOfDeduction.Value,
                        DateOfPayment = payObj.DateOfPayment ?? throw new System.ApplicationException("Date of Payment cannot be empty"),
                        InstallmentID = payObj.InstallmentID,
                        LotNo = payObj.LotNo,
                        NatureOfPaymentID = payObj.NatureOfPaymentID,
                        OwnershipID = payObj.OwnershipID,
                        ReceiptNo = payObj.ReceiptNo,
                        RevisedDateOfPayment = payObj.RevisedDateOfPayment ?? throw new System.ApplicationException("Date of Revised Payment cannot be empty"),
                        TdsRate = GetTaxRate(payObj.TdsTaxCode, payObj.RevisedDateOfPayment.Value),
                        GstRate = GetTaxRate(payObj.GstTaxCode, payObj.RevisedDateOfPayment.Value),
                    };

                    foreach (var trans in payObj.InstallmentList)
                    {
                        var inst = new ClientPaymentTransaction()
                        {
                            OwnershipID = clientPay.OwnershipID,
                            //ClientPaymentID = clientPay.ClientPaymentID,
                            CustomerID = trans.CustomerID,
                            CustomerShare = trans.CustomerShare,
                            InstallmentID = trans.InstallmentID,
                            SellerID = trans.SellerID,
                            SellerShare = trans.SellerShare,
                            Created = DateTime.Now,
                            CreatedBy = "", //todo include customer name after identity implementation
                            RemittanceStatusID = (int)ERemittanceStatus.Pending //todo confirm the status on insert
                        };
                        payObj = PrepareTaxesAndFee(payObj, trans.OwnershipShare);
                        inst.ShareAmount = payObj.ShareAmount;

                        if (payObj.NatureOfPaymentID == 1)
                        {
                            inst.GrossShareAmount = payObj.GrossAmount;
                            inst.LateFee = payObj.LateFee;
                            inst.Gst = payObj.GstAmount;
                            inst.Tds = payObj.TdsAmount;
                            inst.TdsInterest = payObj.TdsInterestAmount;
                            inst.RemittanceStatusID = (int)ERemittanceStatus.Pending;
                        }
                        clientPay.ClientPaymentTransactions.Add(inst);
                    }
                    payments.Add(clientPay);
                }


                try
                {
                    var toinsert = payments;
                    await _context.ClientPayment.AddRangeAsync(payments, cancellationToken);
                    await _context.SaveChangesAsync(cancellationToken);
                    scope.Complete();
                    return Unit.Value;
                }
              
                catch (DbUpdateException e)
                {
                    var sb = new StringBuilder();
                    sb.AppendLine($"DbUpdateException error details - {e?.InnerException?.InnerException?.Message}");

                    foreach (var eve in e.Entries)
                    {
                        sb.AppendLine($"Entity of type {eve.Entity.GetType().Name} in state {eve.State} could not be updated");
                    }

                    throw new Exception(sb.ToString()) ;
                }




            }

            private ClientPaymentDto PrepareTaxesAndFee(ClientPaymentDto cp, decimal share)
            {
                var taxObj = new TaxesAndFees()
                {
                    
                    Share = share,
                    AmountPaid = cp.AmountPaid,
                    DateOfPayment = cp.RevisedDateOfPayment ?? throw new System.ApplicationException("Date of Payment cannot be empty"),
                    DateOfDeduction = cp.DateOfDeduction ?? throw new System.ApplicationException("Date of Deduction cannot be empty"),
                    GstPercentage = GetTaxRate(cp.GstTaxCode, cp.RevisedDateOfPayment.Value),
                    LateFeePerDay = cp.LateFeePerDay ?? throw new System.ApplicationException("Late Fee Per Day cannot be empty"),
                    TdsPercentage = GetTaxRate(cp.TdsTaxCode, cp.RevisedDateOfPayment.Value),
                    IsTdsDeductedBySeller = cp.TdsCollectedBySeller ?? throw new System.ApplicationException("Is TDS Deducted by Seller cannot be empty"),
                };
                var taxes = TaxCalculatorByShare.CalculateTaxAndFees(taxObj);
                cp.GrossAmount = taxes.GrossAmount;
                cp.LateFee = taxes.LateFeeAmount;
                cp.GstAmount = taxes.GSTAmount;
                cp.TdsAmount = taxes.TdsAmount;
                cp.TdsInterestAmount = taxes.TdsInterestAmount;
                cp.ShareAmount = taxes.AmountShare;
                cp.GstRate = taxObj.GstPercentage;
                cp.TdsRate = taxObj.TdsPercentage;
                return cp;
            }

            private decimal GetTaxRate(int taxCode, DateTime revisedDateOfPayment)
            {
                var taxRate = taxCodes
                    .Where(x => LocalDate.FromDateTime(x.EffectiveEndDate) >= LocalDate.FromDateTime(revisedDateOfPayment)
                    && LocalDate.FromDateTime(x.EffectiveStartDate) <= LocalDate.FromDateTime(revisedDateOfPayment))
                    .FirstOrDefault(x => x.TaxCodeId == taxCode);

                return taxRate.TaxValue;
            }

            private DateTime CalculateDeductionDate(DateTime revisedDate) {
                var date = LocalDateTime.FromDateTime(revisedDate.AddMonths(1));
                var lastDate = new DateTime(date.Year, date.Month, DateTime.DaysInMonth(date.Year, date.Month));
                if (DateTime.Now <= lastDate)
                    return revisedDate;
                else
                    return DateTime.Now;
            }
        }
    }
}
