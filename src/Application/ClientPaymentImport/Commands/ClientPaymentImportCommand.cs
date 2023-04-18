using MediatR;
using ReProServices.Application.Common.Interfaces;
using System.Threading.Tasks;
using System.Threading;
using ReProServices.Application.Customers;
using System;
using System.Linq;
using ReProServices.Domain.Enums;
using System.Collections;
using ReProServices.Application.ClientPayments;
using System.Collections.Generic;
using ReProServices.Domain.Entities;
using ReProServices.Application.Common.Models;
using System.Transactions;
using Microsoft.Data.SqlClient;

namespace ReProServices.Application.ClientPaymentImport.Commands
{
    public class ClientPaymentImportCommand : IRequest<Unit>
    {
        public List<ClientPaymentRawImport> cpr { get; set; }

        public class ClientPaymentImportCommandHandler : IRequestHandler<ClientPaymentImportCommand, Unit>
        {
            private readonly IApplicationDbContext _context;
            public ClientPaymentImportCommandHandler(IApplicationDbContext context)
            {
                _context = context;
            }

            public async Task<Unit> Handle(ClientPaymentImportCommand request, CancellationToken cancellationToken)
            {
                try
                {
                    var filtered = (from cr in request.cpr
                                    join cp in _context.CustomerProperty on cr.UnitNo equals cp.UnitNo
                                    join p in _context.Property on cr.PropertyCode equals p.PropertyCode
                                    where cp.StatusTypeId != 3 && cp.PropertyId == p.PropertyID
                                    select cr).Distinct().ToList<ClientPaymentRawImport>();

                    //_context.ClientPaymentRawImport.AddRange(request.cpr);
                    _context.ClientPaymentRawImport.AddRange(filtered);
                    await _context.SaveChangesAsync(cancellationToken);
                    return Unit.Value;

                }
                catch (Exception)
                {

                    throw;
                }
               
            }
        }
    }
}
