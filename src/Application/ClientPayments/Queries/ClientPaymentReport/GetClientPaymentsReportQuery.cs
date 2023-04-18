using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using NodaTime;
using ReProServices.Application.Common.Interfaces;

namespace ReProServices.Application.ClientPayments.Queries.ClientPaymentReport
{
    public class GetClientPaymentsReportQuery : IRequest<IList<Domain.Entities.ClientPaymentReport>>
    {
        public  ClientPaymentFilter Filter { get; set; } = new ClientPaymentFilter();
        public class GetClientPaymentsReportQueryHandler :
                              IRequestHandler<GetClientPaymentsReportQuery, IList<Domain.Entities.ClientPaymentReport>>
        {
            private readonly IApplicationDbContext _context;

            public GetClientPaymentsReportQueryHandler(IApplicationDbContext context)
            {
                _context = context;
            }

            public async Task<IList<Domain.Entities.ClientPaymentReport>> Handle(GetClientPaymentsReportQuery request, CancellationToken cancellationToken)
            {
                var filter = request.Filter;

                try
                {
                    //var vm = (from pay in _context.ViewClientPayment
                    //        join cpt in _context.ClientPaymentTransaction on pay.ClientPaymentID equals cpt.ClientPaymentID
                    //        join cp in _context.ViewCustomerPropertyExpanded on new {cpt.OwnershipID, cpt.CustomerID}
                    //            equals new {cp.OwnershipID, cp.CustomerID}
                    //        join sp in _context.ViewSellerPropertyExpanded on cp.PropertyID equals sp.PropertyID
                    //        join rem in _context.RemittanceStatus on cpt.RemittanceStatusID equals rem.RemittanceStatusID
                    //        join rm in _context.Remittance on cpt.ClientPaymentTransactionID equals rm.ClientPaymentTransactionID
                    //        where cpt.SellerID == sp.SellerID
                    //        select new Domain.Entities.ClientPaymentReport
                    //        {
                    //            OwnershipID = pay.OwnershipID,
                    //            CustomerName = cp.CustomerName,
                    //            Tds = cpt.Tds,
                    //            TdsInterest = cpt.TdsInterest,
                    //            UnitNo = cp.UnitNo,
                    //            LotNo = pay.LotNo,
                    //            TotalUnitCost = cp.TotalUnitCost,
                    //            GrossShareAmount = cpt.GrossShareAmount,
                    //            LateFee = cpt.LateFee,
                    //            DateOfDeduction = pay.DateOfDeduction,
                    //            ShareAmountPaid = cpt.ShareAmount,
                    //            NatureOfPaymentText = pay.NatureOfPaymentID == 1 ? "" : pay.NatureOfPaymentText,
                    //            RevisedDateOfPayment = pay.RevisedDateOfPayment,
                    //            InstallmentID = cpt.InstallmentID,
                    //            DateOfPayment = pay.DateOfPayment,
                    //            ReceiptNo = pay.ReceiptNo,
                    //            Gst = cpt.Gst,
                    //            DateOfBooking = cp.DateOfAgreement.Value, //todo confirm mapping
                    //            PropertyPremises = cp.PropertyPremises,
                    //            SellerName = sp.SellerName,
                    //            ClientPaymentTransactionID = cpt.ClientPaymentTransactionID,
                    //            PropertyID = cp.PropertyID,
                    //            SellerID = sp.SellerID,
                    //            GstRate = pay.GstRate,
                    //            TdsRate = pay.TdsRate,
                    //            RemittanceStatus = rem.RemittanceStatusText,
                    //            NatureOfPaymentID = pay.NatureOfPaymentID,
                    //            RemittanceStatusID = cpt.RemittanceStatusID,
                    //            ChallanDate=rm.ChallanDate
                    //        })
                    //        .PreFilterPaymentsBy(filter)
                    //        .OrderByDescending(x => x.LotNo)
                    //        .ThenBy(_ => _.OwnershipID)
                    //        .ThenByDescending(y => y.ClientPaymentTransactionID)
                    //        .ToList();

                    //vm = vm.AsQueryable().PostFilterPaymentsBy(filter)
                    //    .Select((x,index) => new Domain.Entities.ClientPaymentReport
                    //    {
                    //        SlNo = index + 1,
                    //        CustomerName = x.CustomerName,
                    //        Tds = x.Tds,
                    //        TdsInterest = x.TdsInterest,
                    //        UnitNo = x.UnitNo,
                    //        LotNo = x.LotNo,
                    //        TotalUnitCost = x.TotalUnitCost,
                    //        GrossShareAmount = x.GrossShareAmount,
                    //        LateFee = x.LateFee,
                    //        DateOfDeduction = x.DateOfDeduction,
                    //        ShareAmountPaid = x.ShareAmountPaid,
                    //        NatureOfPaymentText = x.NatureOfPaymentText,
                    //        RevisedDateOfPayment = x.RevisedDateOfPayment,
                    //        InstallmentID = x.InstallmentID,
                    //        DateOfPayment = x.DateOfPayment,
                    //        ReceiptNo = x.ReceiptNo,
                    //        Gst = x.Gst,
                    //        DateOfBooking = x.DateOfBooking, //todo confirm mapping
                    //        PropertyPremises = x.PropertyPremises,
                    //        SellerName = x.SellerName,
                    //        GstRate = x.GstRate,
                    //        TdsRate = x.TdsRate,
                    //        RemittanceStatus = x.RemittanceStatus,
                    //        ClientPaymentTransactionID = x.ClientPaymentTransactionID,
                    //        ChallanDate=x.ChallanDate
                    //    })
                    //    .ToList();

                    var qry = "exec sp_ClientPaymentReport " + filter.NatureOfPaymentID;
                    qry += string.IsNullOrEmpty(filter.CustomerName) ? ",' '" : "'" + filter.CustomerName.ToLower() + "'";
                    qry += filter.PropertyID <= 0 ? ",0" : "," + filter.PropertyID;
                    qry += string.IsNullOrEmpty(filter.Premises) ? ",' '" : "'" + filter.Premises + "'";
                    qry += filter.UnitNo <= 0 ? ",0" : "," + filter.UnitNo;
                    qry += filter.LotNo <= 0 ? ",0" : "," + filter.LotNo;
                    qry += string.IsNullOrEmpty(filter.SellerName) ? ",' '" : ",'" + filter.SellerName + "'";
                    qry += filter.SellerID <= 0 ? ",0" : "," + filter.SellerID;
                    qry += filter.FromRevisedDate.HasValue ? ",'" + LocalDate.FromDateTime(filter.FromRevisedDate.Value) + "'" : ",' '";
                    qry += filter.ToRevisedDate.HasValue ? ",'" + LocalDate.FromDateTime(filter.ToRevisedDate.Value) + "'" : ",' '";
                    qry += filter.RemittanceStatusID.HasValue ? ",'" + filter.RemittanceStatusID.Value + "'" : ",' '";

                    var vm = _context.ViewClientPaymentReport.FromSqlRaw(qry).ToList();
                    var vmFinal = vm.Select((x, index) =>
                             new Domain.Entities.ClientPaymentReport
                             {
                                 SlNo = index + 1,
                                 CustomerName = x.CustomerName,
                                 Tds = x.Tds,
                                 TdsInterest = x.TdsInterest,
                                 UnitNo = x.UnitNo,
                                 LotNo = x.LotNo,
                                 TotalUnitCost = x.TotalUnitCost,
                                 GrossShareAmount = x.GrossShareAmount,
                                 LateFee = x.LateFee,
                                 DateOfDeduction = x.DateOfDeduction,
                                 ShareAmountPaid = x.ShareAmountPaid,
                                 NatureOfPaymentText = x.NatureOfPaymentText,
                                 RevisedDateOfPayment = x.RevisedDateOfPayment,
                                 InstallmentID = x.InstallmentID,
                                 DateOfPayment = x.DateOfPayment,
                                 ReceiptNo = x.ReceiptNo,
                                 Gst = x.Gst,
                                 DateOfBooking = x.DateOfBooking,
                                 PropertyPremises = x.PropertyPremises,
                                 SellerName = x.SellerName,
                                 GstRate = x.GstRate,
                                 TdsRate = x.TdsRate,
                                 RemittanceStatus = x.RemittanceStatus,
                                 ClientPaymentTransactionID = x.ClientPaymentTransactionID,
                                 ChallanDate = x.ChallanDate                                
                             }).OrderByDescending(x => x.LotNo)
                            .ThenBy(_ => _.OwnershipID)
                            .ThenByDescending(y => y.ClientPaymentTransactionID).ToList();
                    return vmFinal;
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
