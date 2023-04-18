using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using ReProServices.Application.Common.Interfaces;

namespace ReProServices.Application.TaxCodes.Command.CopyTaxCodeCommand
{
   public class CopyTaxCodeCommand : IRequest<TaxCodesDto>
    {
        public TaxCodesDto TaxCodeDtoObj { get; set; }

        // ReSharper disable once UnusedMember.Global
        public class CopyTaxCodeCommandHandler : IRequestHandler<CopyTaxCodeCommand, TaxCodesDto>
        {
            private readonly IApplicationDbContext _context;
            private readonly ICurrentUserService _currentUserService;
            public CopyTaxCodeCommandHandler(IApplicationDbContext context, ICurrentUserService currentUserService)
            {
                _context = context;
                _currentUserService = currentUserService;
            }

            public async Task<TaxCodesDto> Handle(CopyTaxCodeCommand request, CancellationToken cancellationToken)
            {
               var taxCodeObj = request.TaxCodeDtoObj;
              
               //if(taxCodeObj.EffectiveStartDate.Date<DateTime.Now.Date)
               //     throw new ApplicationException("This taxCode's effective start date Should not be less than cusrrent date");

                var userInfo = _context.Users.FirstOrDefault(x => x.UserName == _currentUserService.UserName && x.IsActive);
                var existingTax= (from tax in _context.TaxCodes
                 where tax.TaxID == taxCodeObj.TaxID
                 select tax).FirstOrDefault();
               
                if(existingTax==null)
                    throw new ApplicationException("This tax does not exist");
                existingTax.EffectiveEndDate = taxCodeObj.EffectiveStartDate.AddDays(-1);
                existingTax.Updated = DateTime.Now;
                existingTax.UpdatedBy = userInfo.UserID.ToString();
                 _context.TaxCodes.Update(existingTax);

                Domain.Entities.TaxCodes entity = new Domain.Entities.TaxCodes
                {
                    EffectiveEndDate = taxCodeObj.EffectiveEndDate,
                    EffectiveStartDate = taxCodeObj.EffectiveStartDate,
                    TaxName = taxCodeObj.TaxName,
                    TaxTypeId = taxCodeObj.TaxTypeId,
                    TaxValue = taxCodeObj.TaxValue,
                    TaxCodeId = taxCodeObj.TaxCodeId,
                    Created = DateTime.Now,
                    CreatedBy = userInfo.UserID.ToString()
                };
                await _context.TaxCodes.AddAsync(entity, cancellationToken);
                await _context.SaveChangesAsync(cancellationToken);
                return taxCodeObj;
            }
        }
    }
}
