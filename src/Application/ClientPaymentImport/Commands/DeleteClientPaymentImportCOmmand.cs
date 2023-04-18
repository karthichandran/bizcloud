using MediatR;
using ReProServices.Application.Common.Interfaces;
using System.Threading.Tasks;
using System.Threading;
using ReProServices.Application.Customers;
using System;
using ReProServices.Domain.Enums;
using System.Collections;
using ReProServices.Application.ClientPayments;
using System.Collections.Generic;
using ReProServices.Domain.Entities;

namespace ReProServices.Application.ClientPaymentImport.Commands
{
    public class DeleteClientPaymentImportCOmmand : IRequest<bool>
    {
        public class DeleteClientPaymentImportCommandHandler : IRequestHandler<DeleteClientPaymentImportCOmmand,bool> {
            private readonly IApplicationDbContext _context;
            public DeleteClientPaymentImportCommandHandler(IApplicationDbContext context)
            {
                _context = context;
            }

            public async Task<bool> Handle(DeleteClientPaymentImportCOmmand request, CancellationToken cancellationToken) {
                try
                {
                     _context.ClientPaymentRawImport.RemoveRange(_context.ClientPaymentRawImport);
                    await _context.SaveChangesAsync(cancellationToken);
                    return true;

                }
                catch (Exception)
                {

                    throw;
                }
            }
        }
    }
}
