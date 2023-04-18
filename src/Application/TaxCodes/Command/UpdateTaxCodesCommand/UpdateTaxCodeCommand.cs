using System.Threading;
using System.Threading.Tasks;
using MediatR;
using ReProServices.Application.Common.Interfaces;
using System.Linq;
using System;

namespace ReProServices.Application.TaxCodes.Command.UpdateTaxCodesCommand
{
    public class UpdateTaxCodeCommand : IRequest<int>
    {
        public TaxCodesDto TaxCodeDtoObj { get; set; }

        public class UpdateTaxCodeCommandHandler : IRequestHandler<UpdateTaxCodeCommand, int>
        {
            private readonly IApplicationDbContext _context;
            private readonly ICurrentUserService _currentUserService;
            public UpdateTaxCodeCommandHandler(IApplicationDbContext context, ICurrentUserService currentUserService)
            {
                _context = context;
                _currentUserService = currentUserService;
            }

            public async Task<int> Handle(UpdateTaxCodeCommand request, CancellationToken cancellationToken)
            {
                var taxCodeObj = request.TaxCodeDtoObj;

                var overlappingModel = (from tax in _context.TaxCodes
                                        where (tax.EffectiveStartDate <= taxCodeObj.EffectiveStartDate && tax.EffectiveEndDate >= taxCodeObj.EffectiveStartDate) && tax.TaxTypeId == taxCodeObj.TaxTypeId && tax.TaxID != taxCodeObj.TaxID
                                        select tax).ToList();
                if (overlappingModel.Count > 0)
                {
                    throw new ApplicationException("This taxCode's effective start date is overlapping with other Tax code");
                }

                overlappingModel = (from tax in _context.TaxCodes
                                    where tax.EffectiveStartDate <= taxCodeObj.EffectiveEndDate && tax.EffectiveEndDate >= taxCodeObj.EffectiveEndDate && tax.TaxTypeId == taxCodeObj.TaxTypeId && tax.TaxID != taxCodeObj.TaxID
                                    select tax).ToList();
                if (overlappingModel.Count > 0)
                {
                    throw new ApplicationException("This taxCode's effective end date is overlapping with other Tax code");
                }

                var userInfo = _context.Users.FirstOrDefault(x => x.UserName == _currentUserService.UserName && x.IsActive);
                var existingTax = _context.TaxCodes.FirstOrDefault(x => x.TaxID == taxCodeObj.TaxID);

                existingTax.TaxCodeId = taxCodeObj.TaxCodeId;
                existingTax.EffectiveEndDate = taxCodeObj.EffectiveEndDate;
                existingTax.EffectiveStartDate = taxCodeObj.EffectiveStartDate;
                existingTax.TaxName = taxCodeObj.TaxName;
                existingTax.TaxTypeId = taxCodeObj.TaxTypeId;
                existingTax.TaxValue = taxCodeObj.TaxValue;
                existingTax.Updated = DateTime.Now;
                existingTax.UpdatedBy = userInfo.UserID.ToString();            
                _context.TaxCodes.Update(existingTax);
                await _context.SaveChangesAsync(cancellationToken);
                return existingTax.TaxID;
            }
        }
    }
}
