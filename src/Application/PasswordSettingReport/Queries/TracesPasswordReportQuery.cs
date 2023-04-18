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

namespace ReProServices.Application.PasswordSettingReport.Queries
{
    public class TracesPasswordReportQuery :IRequest<IList<TracesPasswordDto>>
    {
        public TracesPasswordReportFilter Filter { get; set; } = new TracesPasswordReportFilter();
        public class TracesPasswordReportQueryHandler : IRequestHandler<TracesPasswordReportQuery, IList<TracesPasswordDto>> {
            private readonly IApplicationDbContext _context;
            public TracesPasswordReportQueryHandler(IApplicationDbContext context)
            {
                _context = context;
            }

            public async Task<IList<TracesPasswordDto>> Handle(TracesPasswordReportQuery request, CancellationToken cancellationToken) {
                try
                {
                    var vm = (from pay in _context.ClientPayment
                              join cpt in _context.ClientPaymentTransaction on pay.ClientPaymentID equals cpt.ClientPaymentID
                              from cp in _context.CustomerProperty.Where(x => x.OwnershipID == cpt.OwnershipID && x.CustomerId == cpt.CustomerID).DefaultIfEmpty()
                              from pt in _context.Property.Where(x => x.PropertyID == cp.PropertyId)
                              from cs in _context.Customer.Where(x => x.CustomerID == cp.CustomerId).DefaultIfEmpty()
                              from rm in _context.Remittance.Where(x => x.ClientPaymentTransactionID == cpt.ClientPaymentTransactionID)
                              select new TracesPasswordDto
                              {
                                  LotNumber = pay.LotNo,
                                  UnitNo = cp.UnitNo.Value,
                                  HasTracesPassword = cs.OnlyTDS.Value ? "Only TDS payment" : cs.IsTracesRegistered ? "Yes" : "No",
                                  Pan = cs.PAN,
                                  DateOfBirth = cs.DateOfBirth,
                                  NameInSystem = cs.Name,
                                  AddressPremises = cs.AddressPremises,
                                  Address1=cs.AdressLine1,
                                  Address2=cs.AddressLine2,
                                  City=cs.City,
                                  Pincode=cs.PinCode,
                                  NameInChallan = rm.ChallanCustomerName,
                                  ChallanSerialNo = rm.ChallanID,
                                  ChallanTotalAmount = rm.ChallanAmount.ToString(),
                                  PropertyId = pt.PropertyID,
                                  PropertyName = pt.AddressPremises
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
