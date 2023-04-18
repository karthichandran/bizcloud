using MediatR;
using AutoMapper;
using ReProServices.Application.Common.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using System;

namespace ReProServices.Application.ClientPayments.Queries
{
    public class GetBasePaymentObjectByOwnershipIDQuery : IRequest<ClientPaymentDto>
    {
        public Guid OwnershipId { get; set; }
        public class GetBasePaymentObjectByOwnershipIDQueryHandler :
                              IRequestHandler<GetBasePaymentObjectByOwnershipIDQuery, ClientPaymentDto>
        {

            private readonly IApplicationDbContext _context;
            private readonly IMapper _mapper;

            public GetBasePaymentObjectByOwnershipIDQueryHandler(IApplicationDbContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            public async Task<ClientPaymentDto> Handle(GetBasePaymentObjectByOwnershipIDQuery request, CancellationToken cancellationToken)
            {
                var installment = new ClientPaymentDto();
                var guid = Guid.NewGuid();

              

                var vm = await (from cp in _context.ViewCustomerPropertyExpanded
                                join sp in _context.ViewSellerPropertyBasic on cp.PropertyID equals sp.PropertyID
                                where cp.OwnershipID == request.OwnershipId
                                select new ClientPaymentRawDto
                                {
                                    CustomerPropertyId = cp.CustomerPropertyID,
                                    CustomerShare = cp.CustomerShare,
                                    CustomerID = cp.CustomerID,
                                    CustomerName = cp.CustomerName,
                                    PAN = cp.CustomerPAN,
                                    SellerID = sp.SellerID,
                                    SellerName = sp.SellerName,
                                    SellerShare = sp.SellerShare,
                                    SellerPropertyId = sp.SellerPropertyID,
                                    PropertyID = cp.PropertyID,
                                    PropertyPremises = cp.PropertyPremises,
                                    UnitNo = cp.UnitNo,
                                    TdsCollectedBySeller = cp.TdsCollectedBySeller,
                                    PaymentMethodID = cp.PaymentMethodId,
                                    GstTaxCode = cp.UnitGstRateID,
                                    TdsTaxCode = cp.UnitTdsRateID,
                                    TotalUnitCost = cp.TotalUnitCost.Value,
                                    OwnershipShare = (sp.SellerShare / 100) * (cp.CustomerShare / 100) * 100,
                                    OwnershipID = cp.OwnershipID,
                                    InstallmentID = guid,
                                    StatusTypeID = cp.StatusTypeID,
                                    PropertyType = cp.PropertyType,
                                    LateFeePerDay = cp.LateFeePerDay,
                                    Remarks = cp.Remarks,
                                    
                                })
                             .ToListAsync(cancellationToken);






                var result = new ClientPaymentVM();
                var vm1 = vm.First();
              

                var cpBase = new ClientPaymentDto
                {
                    CoOwner = vm1.CoOwner,
                    DateOfAgreement = vm1.DateOfAgreement,
                    GstTaxCode = vm1.GstTaxCode,
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
                    PAN = vm1.PAN,
                    TdsCollectedBySeller = vm1.TdsCollectedBySeller,
                    DateOfDeduction = vm1.DateOfDeduction,
                    DateOfPayment = vm1.DateOfPayment,
                    LotNo = vm1.LotNo,
                   
                };

                foreach (var rawObj in vm)
                {
                    var installmentObj = new ClientPaymentTransactionDto()
                    {
                        CustomerID = rawObj.CustomerID,
                        CustomerName = rawObj.CustomerName,
                        CustomerShare = rawObj.CustomerShare,
                        GrossShareAmount = rawObj.GrossAmount,
                        Gst = rawObj.GstAmount,
                        InstallmentID = rawObj.InstallmentID,
                        LateFee = rawObj.LateFee,
                        SellerID = rawObj.SellerID,
                        SellerName = rawObj.SellerName,
                        SellerPropertyId = rawObj.SellerPropertyId,
                        SellerShare = rawObj.SellerShare,
                        ClientPaymentID = rawObj.ClientPaymentID,
                        Tds = rawObj.Tds,
                        OwnershipShare = rawObj.OwnershipShare,
                        //todo check need to include OwnershipID here
                    };
                    cpBase.InstallmentList.Add(installmentObj);
                }



                return cpBase;
            }

        }
    }
}
