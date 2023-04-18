using MediatR;
using ReProServices.Application.Common.Interfaces;
using System.Threading.Tasks;
using System.Threading;
using System.Linq;
using System;

namespace ReProServices.Application.TaxCodes.Command.DeleteTaxCodeCommand
{
    public class DeleteTaxCodeCommand:IRequest<Unit>
    {
        public long TaxId { get; set; }

        public class DeleteTaxCodeCommandHandler : IRequestHandler<DeleteTaxCodeCommand, Unit> {
            private readonly IApplicationDbContext _context;
            public DeleteTaxCodeCommandHandler(IApplicationDbContext context)
            {
                _context = context;
            }
            public async Task<Unit> Handle(DeleteTaxCodeCommand request, CancellationToken cancellationToken) {

                var vm = _context.TaxCodes.FirstOrDefault(x => x.TaxID == request.TaxId);

                if (vm == null) {
                    throw new ApplicationException("Tax Code is not exist");
                }

                var prop = _context.Property.Where(x => x.GstTaxCode == vm.TaxCodeId || x.TDSTaxCode == vm.TaxCodeId).ToList();
                if (prop.Count > 0) {
                    throw new ApplicationException("Tax Code is Referenced in Property");
                }

                var cusProp = _context.CustomerProperty.Where(x => x.GstRateID == vm.TaxCodeId || x.TdsRateID == vm.TaxCodeId).ToList();
                if (cusProp.Count > 0)
                {
                    throw new ApplicationException("Tax Code is Referenced in Customer Details");
                }              

                var taxCode = _context.TaxCodes.First(x => x.TaxID == request.TaxId);                
                _context.TaxCodes.Remove(taxCode);

                // Updating effective start date of next taxCode
                var nextTaxCode = _context.TaxCodes.FirstOrDefault(x => x.TaxID > request.TaxId && x.TaxTypeId == vm.TaxTypeId);
                if (nextTaxCode != null)
                {
                    nextTaxCode.EffectiveStartDate = vm.EffectiveStartDate;
                    _context.TaxCodes.Update(nextTaxCode);
                }

                await _context.SaveChangesAsync(cancellationToken);
                return Unit.Value;
            }
        }

    }
}
