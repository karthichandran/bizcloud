using MediatR;
using ReProServices.Application.Common.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using System;
using System.Collections.Generic;
using ReProServices.Domain.Extensions;

namespace ReProServices.Application.ClientPayments.Queries
{
    public class GetClientPaymentsByOwnershipIdQuery : IRequest<IList<ClientPaymentDto>>
    {
        public Guid OwnershipId { get; set; }
        public class GetClientPaymentsByOwnershipIdQueryHandler :
                              IRequestHandler<GetClientPaymentsByOwnershipIdQuery, IList<ClientPaymentDto>>
        {

            private readonly IApplicationDbContext _context;

            public GetClientPaymentsByOwnershipIdQueryHandler(IApplicationDbContext context)
            {
                _context = context;
            }

            public async Task<IList<ClientPaymentDto>> Handle(GetClientPaymentsByOwnershipIdQuery request, CancellationToken cancellationToken)
            {
                try
                {
                    var payments = new List<ClientPaymentDto>();
                    var installmentListraw = _context.ClientPayment
                                       .Where(x => x.OwnershipID == request.OwnershipId).ToList();

                    if (!installmentListraw.Any())
                    {
                        return payments;
                    }

                    var installmentList = installmentListraw
                        .Select(x => new { x.InstallmentID, x.OwnershipID })
                        .AsEnumerable().DistinctBy(k => k.InstallmentID);

                    var rawList = new List<ClientPaymentRawDto>();
                    foreach (var installmentId in installmentList)
                    {
                        var vm = await (from pay in _context.ViewClientPayment
                                        join cpt in _context.ClientPaymentTransaction on new { pay.InstallmentID, pay.ClientPaymentID } equals new { cpt.InstallmentID, cpt.ClientPaymentID }
                                        join cp in _context.ViewCustomerPropertyExpanded on new { cpt.OwnershipID, cpt.CustomerID } equals new { cp.OwnershipID, cp.CustomerID }
                                        join sp in _context.ViewSellerPropertyExpanded on cp.PropertyID equals sp.PropertyID
                                        where pay.InstallmentID == installmentId.InstallmentID
                                              && cpt.SellerID == sp.SellerID
                                        select new ClientPaymentRawDto
                                        {
                                            CustomerPropertyId = cp.CustomerPropertyID,
                                            CustomerShare = cpt.CustomerShare,
                                            ClientPaymentTransactionID = cpt.ClientPaymentTransactionID,
                                            CustomerID = cpt.CustomerID,
                                            CustomerName = cp.CustomerName,
                                            PAN = cp.CustomerPAN,
                                            SellerID = cpt.SellerID,
                                            SellerName = sp.SellerName,
                                            SellerShare = cpt.SellerShare,
                                            SellerPropertyId = sp.SellerPropertyID,
                                            PropertyID = cp.PropertyID,
                                            PropertyPremises = sp.PropertyPremises,
                                            UnitNo = cp.UnitNo,
                                            TdsCollectedBySeller = cp.TdsCollectedBySeller,
                                            PaymentMethodID = cp.PaymentMethodId,
                                            GstRate = pay.GstRate,
                                            TdsRate = pay.TdsRate,
                                            TotalUnitCost = cp.TotalUnitCost.Value,
                                            OwnershipShare = (sp.SellerShare / 100) * (cpt.CustomerShare / 100) * 100,
                                            OwnershipID = cp.OwnershipID,
                                            InstallmentID = pay.InstallmentID,
                                            StatusTypeID = cp.StatusTypeID,
                                            PropertyType = sp.PropertyType,
                                            LateFeePerDay = sp.LateFeePerDay,
                                            TdsTaxCode = cp.UnitTdsRateID,
                                            GstTaxCode = cp.UnitGstRateID,
                                            GstAmount = cpt.Gst,
                                            TdsInterest = cpt.TdsInterest,
                                            NatureOfPaymentID = pay.NatureOfPaymentID,
                                            AmountPaid = pay.AmountPaid,
                                            DateOfPayment = pay.DateOfPayment,
                                            RevisedDateOfPayment = pay.RevisedDateOfPayment,
                                            DateOfDeduction = pay.DateOfDeduction,
                                            ReceiptNo = pay.ReceiptNo,
                                            LateFee = cpt.LateFee,
                                            ClientPaymentID = pay.ClientPaymentID,
                                            DateOfAgreement = cp.DateOfAgreement,
                                            LotNo = pay.LotNo,
                                            Remarks = cp.Remarks,
                                            GrossShareAmount = cpt.GrossShareAmount,
                                            Tds = cpt.Tds,
                                            NatureofPaymentText = pay.NatureOfPaymentID == 1 ? "" : pay.NatureOfPaymentText,
                                            ShareAmount = cpt.ShareAmount,
                                            RemittanceStatusID = cpt.RemittanceStatusID
                                        })
                                       .ToListAsync(cancellationToken);

                        if (!vm.Any())
                        {
                            throw new ApplicationException("Contact System Admin. Data Integrity issue with installment ID " + installmentId.InstallmentID.ToString());
                        }

                        var vm1 = vm.First();
                        var cpayment = new ClientPaymentDto
                        {
                            CoOwner = vm1.CoOwner,
                           // CustomerPropertyID = vm1.CustomerPropertyId, todo check need for customerpropertyId
                            DateOfAgreement = vm1.DateOfAgreement,
                            InstallmentID = vm1.InstallmentID,
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
                            GstTaxCode = vm1.GstTaxCode,
                            PAN = vm1.PAN,
                            TdsCollectedBySeller = vm1.TdsCollectedBySeller,
                            DateOfDeduction = vm1.DateOfDeduction,
                            DateOfPayment = vm1.DateOfPayment,
                            LotNo = vm1.LotNo,
                            RevisedDateOfPayment = vm1.RevisedDateOfPayment,
                            NatureOfPaymentID = vm1.NatureOfPaymentID,
                            ReceiptNo = vm1.ReceiptNo,
                            AmountPaid = vm1.AmountPaid,
                            ClientPaymentID = vm1.ClientPaymentID,
                            NatureOfPayment = vm1.NatureofPaymentText,
                            GstRate = vm1.GstRate,
                            TdsRate = vm1.TdsRate,
                        };

                        //TODO fixing GST Rate and TDS Rate values
                        foreach (var rawObj in vm)
                        {
                            var installmentObj = new ClientPaymentTransactionDto
                            {
                                ClientPaymentID = rawObj.ClientPaymentID,
                                ShareAmount = rawObj.ShareAmount,
                                CustomerShare = rawObj.CustomerShare,
                                GrossShareAmount = rawObj.GrossShareAmount,
                                Gst = rawObj.GstAmount,
                                InstallmentID = rawObj.InstallmentID,
                                LateFee = rawObj.LateFee,
                                SellerID = rawObj.SellerID,
                                SellerName = rawObj.SellerName,
                                SellerPropertyId = rawObj.SellerPropertyId,
                                SellerShare = rawObj.SellerShare,
                                Tds = rawObj.Tds,
                                TdsInterest = rawObj.TdsInterest,
                                OwnershipShare = rawObj.OwnershipShare,
                                ClientPaymentTransactionID = rawObj.ClientPaymentTransactionID,
                                RemittanceStatusID = rawObj.RemittanceStatusID
                            };
                            installmentObj.CustomerID = rawObj.CustomerID;
                            installmentObj.CustomerName = rawObj.CustomerName;
                            cpayment.InstallmentList.Add(installmentObj);
                        }

                        cpayment.GstAmount = cpayment.InstallmentList.Sum(x => x.Gst);
                        cpayment.TdsAmount = cpayment.InstallmentList.Sum(x => x.Tds);
                        cpayment.TdsInterestAmount = cpayment.InstallmentList.Sum(x => x.TdsInterest);
                        cpayment.GrossAmount = cpayment.InstallmentList.Sum(x => x.GrossShareAmount);
                        cpayment.LateFee = cpayment.InstallmentList.Sum(x => x.LateFee);

                        if (cpayment.NatureOfPaymentID == (int)Domain.Enums.ENatureOfPayment.ToBeConsidered)
                        {
                            cpayment.RoundoffAdjustment = vm1.AmountPaid - cpayment.InstallmentList.Sum(x => x.ShareAmount);
                        }
                        payments.Add(cpayment);
                    }
                   
                    return payments.OrderByDescending(x => x.ClientPaymentID).ToList();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }
            }

        }
    }
}
