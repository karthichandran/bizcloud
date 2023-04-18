using System.Threading;
using System.Threading.Tasks;
using MediatR;
using ReProServices.Application.Common.Interfaces;
using ReProServices.Application.Customers;

namespace ReProServices.Application.CustomerProperty.Commands.DeleteCustomerProperty
{
    public class DeleteCustomerPropertyCommand : IRequest<Unit>
    {
        public CustomerVM CustomerVM { get; set; }

        public class DeleteCustomerPropertyCommandHandler : IRequestHandler<DeleteCustomerPropertyCommand, Unit>
        {
            private readonly IApplicationDbContext _context;
            public DeleteCustomerPropertyCommandHandler(IApplicationDbContext context)
            {
                _context = context;
            }

#pragma warning disable 1998
            public async Task<Unit> Handle(DeleteCustomerPropertyCommand request, CancellationToken cancellationToken)
#pragma warning restore 1998
            {
                //delete customerpropertyfiles
                //delete customerpoeprtyModLog
                //delete customerproprety

                return Unit.Value;
            }
        }
    }
}
