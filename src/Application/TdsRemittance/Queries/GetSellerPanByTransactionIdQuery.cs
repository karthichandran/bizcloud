using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ReProServices.Application.Common.Interfaces;
using ReProServices.Application.TdsRemittance.Queries.GetRemittanceList;
using ReProServices.Domain.Enums;

namespace ReProServices.Application.TdsRemittance
{
    public class GetSellerPanByTransactionIdQuery:IRequest<string>
    {
        public int TransactionID { get; set; }
        public class GetSellerPanByTransactionIdQueryHandler :
                              IRequestHandler<GetSellerPanByTransactionIdQuery, string>
        {

            private readonly IApplicationDbContext _context;

            public GetSellerPanByTransactionIdQueryHandler(IApplicationDbContext context)
            {
                _context = context;
            }

            public async Task<string> Handle(GetSellerPanByTransactionIdQuery request, CancellationToken cancellationToken)
            {

                var pan =( from pay in _context.ClientPayment
                          join cpt in _context.ClientPaymentTransaction on pay.ClientPaymentID equals cpt.ClientPaymentID
                          join cp in _context.ViewCustomerPropertyExpanded on new { cpt.OwnershipID, cpt.CustomerID } equals new { cp.OwnershipID, cp.CustomerID }
                          join sp in _context.ViewSellerPropertyExpanded on cp.PropertyID equals sp.PropertyID
                          join sl in _context.Seller on sp.SellerID equals sl.SellerID
                          where cpt.ClientPaymentTransactionID == request.TransactionID
                          select sl.PAN).ToString();
              
                return pan;
            }

        }
    }
}
