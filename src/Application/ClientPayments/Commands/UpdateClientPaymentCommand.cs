using MediatR;
using ReProServices.Application.Common.Interfaces;
using System.Threading.Tasks;
using System.Threading;
using System;
using ReProServices.Domain.Entities;
using ReProServices.Application.Common.Formulas;
using System.Linq;
using System.Transactions;
using ReProServices.Domain.Enums;

namespace ReProServices.Application.ClientPayments.Commands
{
    public class UpdateClientPaymentCommand : IRequest<Unit>
    {
        public ClientPaymentVM ClientPaymentVM { get; set; }
        public Guid InstallmentID { get; set; }
        public class UpdateClientPaymentCommandHandler : IRequestHandler<UpdateClientPaymentCommand, Unit>
        {
            private readonly IApplicationDbContext _context;
            private readonly ICurrentUserService _currentUserService;
            public UpdateClientPaymentCommandHandler(IApplicationDbContext context, ICurrentUserService currentUserService)
            {
                _context = context;
                _currentUserService = currentUserService;
            }

            public async Task<Unit> Handle(UpdateClientPaymentCommand request, CancellationToken cancellationToken)
            {
                using var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
                var payObj = request.ClientPaymentVM.ExistingInstallments.First(x => x.InstallmentID == request.InstallmentID);
                if (String.IsNullOrEmpty(payObj.ReceiptNo))
                {
                    throw new ApplicationException("Empty Receipt Number");
                }
                var userInfo = _context.Users.FirstOrDefault(x => x.UserName == _currentUserService.UserName && x.IsActive);
                var entity = _context.ClientPayment.First(x => x.ClientPaymentID == payObj.ClientPaymentID);
                entity.ClientPaymentID = payObj.ClientPaymentID;
                entity.AmountPaid = payObj.AmountPaid;
                entity.DateOfDeduction = payObj.DateOfDeduction?? throw new ApplicationException("Date of Deduction cannot be empty");
                entity.DateOfPayment = payObj.DateOfPayment ?? throw new ApplicationException("Date of Payment cannot be empty");
                entity.InstallmentID = payObj.InstallmentID;
                entity.LotNo = payObj.LotNo;
                entity.NatureOfPaymentID = payObj.NatureOfPaymentID;
                entity.ReceiptNo = payObj.ReceiptNo;
                entity.RevisedDateOfPayment = payObj.RevisedDateOfPayment ?? throw new ApplicationException("Revised Date of Payment cannot be empty");
                entity.TdsRate = GetTaxRate(payObj.TdsTaxCode, payObj.RevisedDateOfPayment.Value);
                entity.GstRate = GetTaxRate(payObj.GstTaxCode, payObj.RevisedDateOfPayment.Value);
                entity.Updated = DateTime.Now;
                entity.UpdatedBy = userInfo.UserID.ToString();
                _ = _context.ClientPayment.Update(entity);
                await _context.SaveChangesAsync(cancellationToken);
                    
                foreach (var trans in payObj.InstallmentList)
                {
                    var isntEntity = _context.ClientPaymentTransaction
                        .First(x => x.ClientPaymentTransactionID == trans.ClientPaymentTransactionID);
                    isntEntity.ClientPaymentTransactionID = trans.ClientPaymentTransactionID;
                    isntEntity.ClientPaymentID = entity.ClientPaymentID;
                    isntEntity.CustomerID = trans.CustomerID;
                    isntEntity.CustomerShare = trans.CustomerShare;
                    isntEntity.SellerID = trans.SellerID;
                    isntEntity.SellerShare = trans.SellerShare;
                    isntEntity.RemittanceStatusID = (int) ERemittanceStatus.Ignore;
                    payObj = PrepareTaxesAndFee(payObj, trans.OwnershipShare);
                    isntEntity.ShareAmount = payObj.ShareAmount;

                    if (payObj.NatureOfPaymentID == (int)ENatureOfPayment.ToBeConsidered)
                    {
                        isntEntity.GrossShareAmount = payObj.GrossAmount;
                        isntEntity.LateFee = payObj.LateFee;
                        isntEntity.Gst = payObj.GstAmount;
                        isntEntity.Tds = payObj.TdsAmount;
                        isntEntity.TdsInterest = payObj.TdsInterestAmount;
                        isntEntity.RemittanceStatusID = (int)ERemittanceStatus.Pending;
                    }
                    isntEntity.Updated = DateTime.Now;
                    isntEntity.UpdatedBy = userInfo.UserID.ToString();
                    _ = _context.ClientPaymentTransaction.Update(isntEntity);
                    await _context.SaveChangesAsync(cancellationToken);
                }
                scope.Complete();
                return Unit.Value;
            }

            private ClientPaymentDto PrepareTaxesAndFee(ClientPaymentDto cp, decimal share)
            {
                var taxObj = new TaxesAndFees()
                {
                    Share = share,
                    AmountPaid = cp.AmountPaid,
                    DateOfPayment = cp.RevisedDateOfPayment ?? throw new ApplicationException("Revised Date of Payment cannot be empty"),
                    DateOfDeduction = cp.DateOfDeduction ?? throw new ApplicationException("Date of Deduction cannot be empty"),
                    GstPercentage = GetTaxRate(cp.GstTaxCode, cp.RevisedDateOfPayment.Value),
                    LateFeePerDay = cp.LateFeePerDay ?? throw new ApplicationException("Late fee per day cannot be empty"),
                    TdsPercentage = GetTaxRate(cp.TdsTaxCode, cp.RevisedDateOfPayment.Value),
                    IsTdsDeductedBySeller = cp.TdsCollectedBySeller ?? throw new ApplicationException("TDS Collected By Seller setting cannot be null"),
                };
                var taxes = TaxCalculatorByShare.CalculateTaxAndFees(taxObj);
                cp.GrossAmount = taxes.GrossAmount;
                cp.LateFee = taxes.LateFeeAmount;
                cp.GstAmount = taxes.GSTAmount;
                cp.TdsAmount = taxes.TdsAmount;
                cp.TdsInterestAmount = taxes.TdsInterestAmount;
                cp.ShareAmount = taxes.AmountShare;
                return cp;
            }

            private decimal GetTaxRate(int taxCode, DateTime revisedDateOfPayment)
            {
                var taxRate = _context.TaxCodes
                    .Where( x=> x.EffectiveEndDate >= revisedDateOfPayment && x.EffectiveStartDate <= revisedDateOfPayment)
                    .First(x => x.TaxCodeId == taxCode).TaxValue;
                return taxRate;
            }
        }
    }
}
