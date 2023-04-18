using System;
using AutoMapper;
using MediatR;
using ReProServices.Application.Common.Interfaces;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using ReProServices.Domain.Entities;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace ReProServices.Application.Customers.Queries
{
    public class GetCustomersQuery : IRequest<CustomerVM>
    {
        public CustomerDetailsFilter Filter { get; set; } = new CustomerDetailsFilter();
        public class GetCustomersQueryHandler : IRequestHandler<GetCustomersQuery, CustomerVM>
        {
            private readonly IApplicationDbContext _context;
            private readonly IMapper _mapper;
            private readonly ILogger<GetCustomersQueryHandler> _logger;
            public GetCustomersQueryHandler(IApplicationDbContext context, IMapper mapper, ILogger<GetCustomersQueryHandler> logger)
            {
                _context = context;
                _mapper = mapper;
                _logger = logger;
            }

            public async Task<CustomerVM> Handle(GetCustomersQuery request, CancellationToken cancellationToken)
            {
                var filter = request.Filter;

                var qry = "exec sp_CustomerReport ";
                qry += string.IsNullOrEmpty(filter.CustomerName) ? "' '" : "'" + filter.CustomerName + "'";
                qry += string.IsNullOrEmpty(filter.PAN) ? ",' '" : ",'" + filter.PAN.ToUpper() + "'";
                qry += filter.PropertyId <= 0 ? ",0" : "," + filter.PropertyId;
                qry += string.IsNullOrEmpty(filter.Premises) ? ",' '" : ",'" + filter.Premises + "'";
                qry += filter.UnitNo <= 0 ? ",0" : "," + filter.UnitNo;
                qry += filter.StatusTypeId <= 0 ? ",0" : "," + filter.StatusTypeId;
                qry += string.IsNullOrEmpty(filter.Remarks) ? ",' '" : ",'" + filter.Remarks + "'";

                var vm = _context.ViewCustomerReports.FromSqlRaw(qry).ToList();
                var vm1 = vm.GroupBy(g => g.OwnershipID)
                   .Select(x => new ViewCustomerReport
                   {
                       PropertyPremises = x.First().PropertyPremises,
                       CustomerName = string.Join(",", x.Select(g => g.CustomerName)),
                       PAN = string.Join(",", x.Select(g => g.PAN)),
                       DateOfAgreement = x.First().DateOfAgreement,
                       OwnershipID = x.First().OwnershipID,
                       UnitNo = x.First().UnitNo,
                       CustomerID = x.First().CustomerID,
                       CustomerPropertyID = x.First().CustomerPropertyID,
                       DateOfSubmission = x.First().DateOfSubmission,
                       PaymentMethodId = x.First().PaymentMethodId,
                       PropertyID = x.First().PropertyID,
                       Remarks = x.First().Remarks,
                       StatusTypeID = x.First().StatusTypeID,
                       TotalUnitCost = x.First().TotalUnitCost,
                       TracesPassword = string.Join(",", x.Select(g => g.TracesPassword)),
                       CustomerAlias = x.First().CustomerAlias,
                       UnitStatus = x.First().UnitStatus,
                       StampDuty = x.First().StampDuty,
                       IncomeTaxPassword = string.Join(",", x.Select(g => g.IncomeTaxPassword))
                   });

                CustomerVM customerVM = new CustomerVM();
                List<ViewCustomerPropertyBasic> cpList=new List<ViewCustomerPropertyBasic>();
                foreach (var obj in vm1) {
                    cpList.Add(new ViewCustomerPropertyBasic {
                        PropertyPremises =obj.PropertyPremises,
                        CustomerName = obj.CustomerName,
                        PAN = obj.PAN,
                        DateOfAgreement = obj.DateOfAgreement,
                        OwnershipID = obj.OwnershipID,
                        UnitNo = obj.UnitNo,
                        CustomerID = obj.CustomerID,
                        CustomerPropertyID = obj.CustomerPropertyID,
                        DateOfSubmission = obj.DateOfSubmission,
                        PaymentMethodId = obj.PaymentMethodId,
                        PropertyID = obj.PropertyID,
                        Remarks = obj.Remarks,
                        StatusTypeID = obj.StatusTypeID,
                        TotalUnitCost = obj.TotalUnitCost,
                        TracesPassword = obj.TracesPassword,
                        CustomerAlias = obj.CustomerAlias,
                        UnitStatus = obj.UnitStatus,
                        StampDuty = obj.StampDuty,
                        IncomeTaxPassword = obj.IncomeTaxPassword
                    });
                }
               


                //if (request.Filter.StatusTypeId == 7) {
                //    cpList = _context.ViewCustomerPropertyArchived.ToList()
                //   .GroupBy(g => g.OwnershipID)
                //   .Select(x => new ViewCustomerPropertyBasic
                //   {
                //       PropertyPremises = x.First().PropertyPremises,
                //       CustomerName = string.Join(",", x.Select(g => g.CustomerName)),
                //       PAN = string.Join(",", x.Select(g => g.PAN)),
                //       DateOfAgreement = x.First().DateOfAgreement,
                //       OwnershipID = x.First().OwnershipID,
                //       UnitNo = x.First().UnitNo,
                //       CustomerID = x.First().CustomerID,
                //       CustomerPropertyID = x.First().CustomerPropertyID,
                //       DateOfSubmission = x.First().DateOfSubmission,
                //       PaymentMethodId = x.First().PaymentMethodId,
                //       PropertyID = x.First().PropertyID,
                //       Remarks = x.First().Remarks,
                //       StatusTypeID = x.First().StatusTypeID,
                //       TotalUnitCost = x.First().TotalUnitCost,
                //       TracesPassword = string.Join(",", x.Select(g => g.TracesPassword)),
                //       CustomerAlias=x.First().CustomerAlias,
                //       UnitStatus=x.First().UnitStatus,
                //       StampDuty=x.First().StampDuty,
                //       IncomeTaxPassword = string.Join(",", x.Select(g => string.IsNullOrEmpty(g.IncomeTaxPassword) ? "No" : "Yes"))
                //   }).AsQueryable()
                //  .FilterCustomersBy(request.Filter).ToList();
                //}
                //else
                // cpList = _context.ViewCustomerPropertyBasic.ToList()
                //    .GroupBy(g => g.OwnershipID)
                //    .Select(x => new ViewCustomerPropertyBasic
                //    {
                //        PropertyPremises = x.First().PropertyPremises,
                //        CustomerName = string.Join(",", x.Select(g => g.CustomerName)),
                //        PAN = string.Join(",", x.Select(g => g.PAN)),
                //        DateOfAgreement = x.First().DateOfAgreement,
                //        OwnershipID = x.First().OwnershipID,
                //        UnitNo = x.First().UnitNo,
                //        CustomerID = x.First().CustomerID,
                //        CustomerPropertyID = x.First().CustomerPropertyID,
                //        DateOfSubmission = x.First().DateOfSubmission,
                //        PaymentMethodId = x.First().PaymentMethodId,
                //        PropertyID = x.First().PropertyID,
                //        Remarks = x.First().Remarks,
                //        StatusTypeID = x.First().StatusTypeID,
                //        TotalUnitCost = x.First().TotalUnitCost,
                //        TracesPassword= string.Join(",", x.Select(g => g.TracesPassword)),
                //        CustomerAlias= x.First().CustomerAlias,
                //        UnitStatus = x.First().UnitStatus,
                //        StampDuty = x.First().StampDuty,
                //        IncomeTaxPassword= string.Join(",", x.Select(g => string.IsNullOrEmpty(g.IncomeTaxPassword)?"No":"Yes"))
                //    }).AsQueryable()         
                //   .FilterCustomersBy(request.Filter).ToList();

                try
                {
                    var withoutProp = _context.ViewCustomerWithoutProperty
                        .Select(x => new ViewCustomerPropertyBasic
                        {
                            CustomerName = x.CustomerName,
                            PAN = x.PAN,
                            CustomerID = x.CustomerID
                        })
                        .ToList()
                        .AsQueryable().FilterCustomersBy(request.Filter).ToList();

                  cpList.AddRange(withoutProp);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }
                
                customerVM.customersView = cpList;
                return customerVM;
            }
        }
    }
}

