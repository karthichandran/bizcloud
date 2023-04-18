using MediatR;
using ReProServices.Application.Common.Interfaces;
using System.Threading;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using ReProServices.Domain.Enums;

namespace ReProServices.Application.ClientPayments.Queries
{
    public class GetClientPaymentsListFromImportsQuery : IRequest<List<ClientPaymentDto>>
    {
        public class GetClientPaymentsListFromImportsQueryHandler :
                              IRequestHandler<GetClientPaymentsListFromImportsQuery, List<ClientPaymentDto>>
        {

            private readonly IApplicationDbContext _context;

            public GetClientPaymentsListFromImportsQueryHandler(IApplicationDbContext context)
            {
                _context = context;
            }

#pragma warning disable 1998
            public async Task<List<ClientPaymentDto>> Handle(GetClientPaymentsListFromImportsQuery request, CancellationToken cancellationToken)
#pragma warning restore 1998
            {
                try
                {
                    var payments = new List<ClientPaymentDto>();

                    

                    Dictionary<string, List<ClientPaymentRawDto>> myDictionary = (from cp in _context.ViewClientPaymentImports
                                                                                  select new ClientPaymentRawDto
                                                                                  {
                                                                                      CustomerPropertyId = cp.CustomerPropertyID,
                                                                                      CustomerShare = cp.CustomerShare,
                                                                                      CustomerID = cp.CustomerID,
                                                                                      CustomerName = cp.CustomerName,
                                                                                      PAN = cp.CustomerPAN,
                                                                                      SellerID = cp.SellerID,
                                                                                      SellerName = cp.SellerName,
                                                                                      SellerShare = cp.SellerShare,
                                                                                      SellerPropertyId = cp.SellerPropertyID,
                                                                                      PropertyID = cp.PropertyID,
                                                                                      PropertyPremises = cp.PropertyPremises,
                                                                                      UnitNo = cp.UnitNo,
                                                                                      TdsCollectedBySeller = cp.TdsCollectedBySeller,
                                                                                      PaymentMethodID = cp.PaymentMethodID,
                                                                                      GstTaxCode = cp.GstRateID,
                                                                                      TdsTaxCode = cp.TdsRateID,
                                                                                      TotalUnitCost =  cp.TotalUnitCost,
                                                                                      OwnershipShare = (cp.SellerShare / 100) * (cp.CustomerShare / 100) * 100,
                                                                                      OwnershipID = cp.OwnershipID,
                                                                                      StatusTypeID = cp.StatusTypeID,
                                                                                      PropertyType = cp.PropertyType,
                                                                                      LateFeePerDay = cp.LateFeePerDay,

                                                                                      AmountPaid = cp.ReceiptTotal,
                                                                                      ReceiptNo = cp.ReceiptNo,
                                                                                      DateOfPayment = cp.DateOfPayment,
                                                                                      RevisedDateOfPayment = cp.RevisedDateOfPayment,
                                                                                      DateOfDeduction = cp.RevisedDateOfPayment,
                                                                                      CoOwner = cp.CoOwner,
                                                                                      DateOfAgreement =  cp.DateOfAgreement,
                                                                                      LotNo = cp.LotNo,
                                                                                      NatureOfPaymentID = GetNatureOfPaymentID(cp.NatureOfPayment, cp.NotToBeConsideredReason),


                                                                                      // NatureOfPaymentID = 1 //todo remove hardcoded value
                                                                                  }).ToList()
                           .GroupBy(o => (o.PropertyID + '-' + o.ReceiptNo))
                           .ToDictionary(g => g.Key, g => g.ToList());


                    foreach (var s in myDictionary.Keys)
                    {
                        var vm = myDictionary[s];
                        var vm1 = vm.First();
                        var installmentID = Guid.NewGuid();
                        var cpBase = new ClientPaymentDto
                        {
                            CoOwner = vm1.CoOwner,
                            DateOfAgreement = vm1.DateOfAgreement,
                            GstTaxCode = vm1.GstTaxCode,
                            InstallmentID = installmentID,
                            LateFeePerDay = vm1.LateFeePerDay,
                            OwnershipID = vm1.OwnershipID,
                            PropertyID = vm1.PropertyID,
                            PropertyPremises = vm1.PropertyPremises,
                            PropertyType = vm1.PropertyType,
                            Remarks = vm1.Remarks,
                            TdsTaxCode = vm1.TdsTaxCode,
                            TotalUnitCost = vm1.TotalUnitCost,
                            UnitNo = vm1.UnitNo,
                            StatusTypeID = vm1.StatusTypeID,
                            PAN = vm1.PAN,
                            TdsCollectedBySeller = vm1.TdsCollectedBySeller,
                            DateOfDeduction = vm1.DateOfDeduction,
                            DateOfPayment = vm1.DateOfPayment,
                            RevisedDateOfPayment = vm1.RevisedDateOfPayment,
                            LotNo = vm1.LotNo,
                            AmountPaid = vm1.AmountPaid,
                            ReceiptNo = vm1.ReceiptNo,
                            NatureOfPaymentID = vm1.NatureOfPaymentID,
                        };

                        foreach (var rawObj in vm)
                        {
                            var installmentObj = new ClientPaymentTransactionDto()
                            {
                                CustomerID = rawObj.CustomerID,
                                CustomerName = rawObj.CustomerName,
                                CustomerShare = rawObj.CustomerShare,
                                InstallmentID = installmentID,
                                SellerID = rawObj.SellerID,
                                SellerName = rawObj.SellerName,
                                SellerPropertyId = rawObj.SellerPropertyId,
                                SellerShare = rawObj.SellerShare,
                                OwnershipShare = rawObj.OwnershipShare,
                                OwnershipID = rawObj.OwnershipID
                            };
                            cpBase.InstallmentList.Add(installmentObj);
                        }

                        payments.Add(cpBase);
                    }
                    return payments;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            private static int GetNatureOfPaymentID(string natureOfPay, string notToBeConsideredReason)
            {
                natureOfPay = natureOfPay.Replace(" ", string.Empty).Trim().ToLower();

                if (string.IsNullOrEmpty(natureOfPay))
                {
                    return (int)ENatureOfPayment.ToBeConsidered;
                }

                if (natureOfPay == "tobeconsidered")
                    return (int)ENatureOfPayment.ToBeConsidered;

                notToBeConsideredReason = notToBeConsideredReason.Replace(" ", string.Empty).Trim().ToLower();
                switch (notToBeConsideredReason)
                {
                    case "chequebounce":
                        return (int)ENatureOfPayment.ChequeBounce;
                    case "frankingcharges":
                        return (int)ENatureOfPayment.FrankingCharges;
                    case "nottobeconsidered":
                        return (int)ENatureOfPayment.NotToBeConsidered;
                    case "tdsamount":
                        return (int)ENatureOfPayment.TDSAmount;
                    case "tdspaidbycustomer":
                        return (int)ENatureOfPayment.TDSPaidByCustomer;
                    default:
                        throw new ApplicationException("Unidentified nature of payment type");
                }
            }
        }
    }
}
