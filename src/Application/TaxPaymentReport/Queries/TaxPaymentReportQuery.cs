using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ReProServices.Application.Common.Interfaces;

namespace ReProServices.Application.TaxPaymentReport.Queries
{
    public class TaxPaymentReportQuery : IRequest<IList<TaxPaymentDto>>
    {
        public TaxPaymentReportFilter Filter { get; set; } = new TaxPaymentReportFilter();
        public class TaxPaymentReportQueryHandler : IRequestHandler<TaxPaymentReportQuery, IList<TaxPaymentDto>> {
            private readonly IApplicationDbContext _context;
            public TaxPaymentReportQueryHandler(IApplicationDbContext context)
            {
                _context = context;
            }

            public async Task<IList<TaxPaymentDto>> Handle(TaxPaymentReportQuery request, CancellationToken cancellationToken) {
                try
                {
                    var vm = (from pay in _context.ClientPayment
                              join cpt in _context.ClientPaymentTransaction on pay.ClientPaymentID equals cpt.ClientPaymentID
                              from cp in _context.CustomerProperty.Where(x => x.OwnershipID == cpt.OwnershipID && x.CustomerId == cpt.CustomerID).DefaultIfEmpty()
                              from pt in _context.Property.Where(x => x.PropertyID == cp.PropertyId)
                              from cs in _context.Customer.Where(x => x.CustomerID == cp.CustomerId).DefaultIfEmpty()
                              from rm in _context.Remittance.Where(x => x.ClientPaymentTransactionID == cpt.ClientPaymentTransactionID)
                              select new TaxPaymentDto
                              {
                                  LotNumber = pay.LotNo,
                                  UnitNo = cp.UnitNo.Value,
                                  NameInChallan = rm.ChallanCustomerName,
                                  ChallanSerialNo = rm.ChallanID,
                                  ChallanPaymentDate=rm.ChallanDate,
                                  ChallanIncomeTaxAmount=rm.ChallanIncomeTaxAmount.ToString(),
                                  ChallanInterestAmount=rm.ChallanInterestAmount.ToString(),
                                  ChallanFeeAmount=rm.ChallanFeeAmount.ToString(),
                                  ChallanTotalAmount = rm.ChallanAmount.ToString(),
                                  PropertyId = pt.PropertyID,
                                  PropertyName = pt.AddressPremises,
                                  CustomerName=cs.Name
                              }
                              ).PreFilterReportBy(request.Filter).ToList();
                    return vm;
                }
                catch (Exception ex) {
                    throw ex;
                }
            }
        }
    }
}
