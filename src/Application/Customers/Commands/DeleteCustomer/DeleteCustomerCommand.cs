using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using ReProServices.Application.Common.Exceptions;
using ReProServices.Application.Common.Interfaces;

namespace ReProServices.Application.Customers.Commands.DeleteCustomer
{
    public class DeleteCustomerCommand : IRequest<Unit>
    {
        public int CustomerID { get; set; }
        public Guid? OwnershipID { get; set; }

        public class DeleteCustomerCommandHandler : IRequestHandler<DeleteCustomerCommand, Unit>
        {
            private readonly IApplicationDbContext _context;
            public DeleteCustomerCommandHandler(IApplicationDbContext context)
            {
                _context = context;
            }

            public async Task<Unit> Handle(DeleteCustomerCommand request, CancellationToken cancellationToken)
            {
                bool hasOwnershidId = false;
                if (request.OwnershipID.Value != Guid.Empty)
                    hasOwnershidId = true;
                
               List<Domain.Entities.CustomerProperty> propertiesByCustomer =null;
                if (hasOwnershidId)                    
                 propertiesByCustomer = _context.CustomerProperty.Where(x => x.CustomerId == request.CustomerID && x.OwnershipID==request.OwnershipID).ToList();
                else
                 propertiesByCustomer = _context.CustomerProperty.Where(x => x.CustomerId == request.CustomerID).ToList();
                //todo confirm to delete property assigned and delete properties assigned

                //if (propertiesByCustomer.Any())
                //{ throw new ApplicationException("Cannot Delete Customer. Customer has Properties assigned"); }
               
                if (!propertiesByCustomer.Any()) {
                    var cust = _context.Customer.First(x => x.CustomerID == request.CustomerID);
                    _ = _context.Customer.Remove(cust);
                    await _context.SaveChangesAsync(cancellationToken);
                }
                else
                {
                    var customerbilling = _context.CustomerBilling.Where(x => x.CustomerID == request.CustomerID && x.OwnershipID == propertiesByCustomer[0].OwnershipID).ToList();
                    if(customerbilling.Any())
                        throw new ApplicationException("Cannot Delete Customer. Customer has Billing Records");

                    var clientPaymentTrans = _context.ClientPaymentTransaction.Where(x => x.CustomerID == request.CustomerID && x.OwnershipID == propertiesByCustomer[0].OwnershipID).ToList();
                    if(clientPaymentTrans.Any())
                        throw new ApplicationException("Cannot Delete Customer. Customer has Payment transaction");

                    //var customerProperty= _context.CustomerProperty.Where(x => x.CustomerId == request.CustomerID).ToList();
                    //if (customerProperty.Count == 1)
                    //{
                    //    var cust = _context.Customer.First(x => x.CustomerID == request.CustomerID);
                    //    _ = _context.Customer.Remove(cust);
                    //    await _context.SaveChangesAsync(cancellationToken);
                    //}
                    //var custProp=_context.CustomerProperty.First(x => x.CustomerId == request.CustomerID && x.OwnershipID == request.OwnershipID);
                    //_ = _context.CustomerProperty.Remove(custProp);
                    //await _context.SaveChangesAsync(cancellationToken);

                    var customerProperty = _context.CustomerProperty.Where(x => x.OwnershipID == request.OwnershipID).ToList();
                    if (customerProperty.Count == 1)
                    {
                        var cust = _context.Customer.First(x => x.CustomerID == request.CustomerID);
                        _ = _context.Customer.Remove(cust);

                    }
                    var custProp = _context.CustomerProperty.First(x => x.CustomerId == request.CustomerID && x.OwnershipID == request.OwnershipID);
                    _ = _context.CustomerProperty.Remove(custProp);
                    await _context.SaveChangesAsync(cancellationToken);
                }
               

                return  Unit.Value;
            }
        }
    }
}
