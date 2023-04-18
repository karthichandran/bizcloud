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

namespace ReProServices.Application.ClientPayments.Commands
{
    public class CreateClientPaymentCommand : IRequest<Unit>
    {
        public ClientPaymentVM ClientPaymentVM { get; set; }

        public class CreateClientPaymentCommandHandler : IRequestHandler<CreateClientPaymentCommand, Unit>
        {
            private readonly IApplicationDbContext _context;
            private readonly ICurrentUserService _currentUserService;
            public CreateClientPaymentCommandHandler(IApplicationDbContext context,ICurrentUserService currentUserService)
            {
                _context = context;
                _currentUserService = currentUserService;
            }

            public async Task<Unit> Handle(CreateClientPaymentCommand request, CancellationToken cancellationToken)
            {
                try
                {
                var payObj = request.ClientPaymentVM.InstallmentBaseObject;

                foreach (var installment in payObj.InstallmentList)
                {
                    if (new ClientPaymentCommandHelper(_context).CheckIfDuplicateReceipt(installment.SellerID,  payObj.ReceiptNo))
                        throw new ApplicationException("Duplicate Receipt No :" + payObj.ReceiptNo);
                }

                using var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
                    var userInfo = _context.Users.FirstOrDefault(x => x.UserName == _currentUserService.UserName && x.IsActive);
                    var clientPay = new ClientPayment
                {
                    AmountPaid = payObj.AmountPaid,
                    Created = DateTime.Now,
                    CreatedBy = userInfo.UserID.ToString(),
                    DateOfDeduction = payObj.DateOfDeduction?? throw new System.ApplicationException("Date of Deduction cannot be empty"),
                    DateOfPayment = payObj.DateOfPayment?? throw new System.ApplicationException("Date of Payment cannot be empty"),
                    InstallmentID = payObj.InstallmentID,
                    LotNo = payObj.LotNo,
                    NatureOfPaymentID = payObj.NatureOfPaymentID,
                    OwnershipID = payObj.OwnershipID,
                    ReceiptNo = payObj.ReceiptNo,
                    RevisedDateOfPayment = payObj.RevisedDateOfPayment ?? throw new System.ApplicationException("Date of Revised Payment cannot be empty"),
                    TdsRate = GetTaxRate(payObj.TdsTaxCode, payObj.RevisedDateOfPayment.Value) ,
                    GstRate = GetTaxRate(payObj.GstTaxCode, payObj.RevisedDateOfPayment.Value),
                    
                };

                

                foreach (var trans in payObj.InstallmentList)
                {
                    var inst = new ClientPaymentTransaction()
                    {
                        OwnershipID = clientPay.OwnershipID,
                        ClientPaymentID = clientPay.ClientPaymentID,
                        CustomerID = trans.CustomerID,
                        CustomerShare = trans.CustomerShare,
                        InstallmentID = trans.InstallmentID,
                        SellerID = trans.SellerID,
                        SellerShare = trans.SellerShare,
                        Created = DateTime.Now,
                        CreatedBy = userInfo.UserID.ToString(),
                        RemittanceStatusID = (int)ERemittanceStatus.Ignore
                    };
                    payObj = PrepareTaxesAndFee(payObj, trans.OwnershipShare);
                    inst.ShareAmount = payObj.ShareAmount;

                    if (payObj.NatureOfPaymentID == 1)
                    {
                        inst.GrossShareAmount = payObj.GrossAmount;
                        inst.LateFee = payObj.LateFee;
                        inst.Gst = payObj.GstAmount;
                        inst.Tds = payObj.TdsAmount;
                        inst.TdsInterest =  payObj.TdsInterestAmount;
                        inst.RemittanceStatusID = (int) ERemittanceStatus.Pending;
                    }
                    clientPay.ClientPaymentTransactions.Add(inst);
                }

                await _context.ClientPayment.AddAsync(clientPay, cancellationToken);
                await _context.SaveChangesAsync(cancellationToken);

                scope.Complete();
                return Unit.Value;

                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.InnerException);
                    throw;
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
                var taxRate = _context.TaxCodes
                    .Where(x => x.EffectiveEndDate >= revisedDateOfPayment && x.EffectiveStartDate <= revisedDateOfPayment)
                    .First(x => x.TaxCodeId == taxCode).TaxValue;
                return taxRate;
            }
        }
    }
}
