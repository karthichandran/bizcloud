using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using ReProServices.Application.Common.Interfaces;

namespace ReProServices.Application.TaxCodes.Command.CreateTaxCodesCommand
{
    public class CreateTaxCodeCommand : IRequest<int>
    {
        public TaxCodesDto TaxCodeDtoObj { get; set; }

        // ReSharper disable once UnusedMember.Global
        public class CreateTaxCodeCommandHandler : IRequestHandler<CreateTaxCodeCommand, int>
        {
            private readonly IApplicationDbContext _context;
            private readonly ICurrentUserService _currentUserService;
            public CreateTaxCodeCommandHandler(IApplicationDbContext context, ICurrentUserService currentUserService)
            {
                _context = context;
                _currentUserService = currentUserService;
            }

            public async Task<int> Handle(CreateTaxCodeCommand request, CancellationToken cancellationToken)
            {
                var taxCodeObj = request.TaxCodeDtoObj;
                var overlappingModel = (from tax in _context.TaxCodes
                                       where tax.EffectiveStartDate <= taxCodeObj.EffectiveStartDate && tax.EffectiveEndDate >= taxCodeObj.EffectiveStartDate && tax.TaxTypeId==taxCodeObj.TaxTypeId
                                       select tax).ToList();
                if (overlappingModel.Count >0)
                {
                    throw new ApplicationException("This taxCode's effective start date is overlapping with other Tax code");
                }

                overlappingModel = (from tax in _context.TaxCodes
                                   where tax.EffectiveStartDate <= taxCodeObj.EffectiveEndDate && tax.EffectiveEndDate >= taxCodeObj.EffectiveEndDate  && tax.TaxTypeId == taxCodeObj.TaxTypeId
                                   select tax).ToList();
                if (overlappingModel.Count >0)
                {
                    throw new ApplicationException("This taxCode's effective end date is overlapping with other Tax code");
                }

                // var maxDate = _context.TaxCodes.Where(x=>x.TaxTypeId==taxCodeObj.TaxTypeId).Max(x => x.EffectiveEndDate);
                var taxCodeByType = _context.TaxCodes.Where(x => x.TaxTypeId == taxCodeObj.TaxTypeId).ToList();
                if (taxCodeByType.Count > 0)
                {
                    var maxDate = taxCodeByType.Max(x => x.EffectiveEndDate);

                    var days = taxCodeObj.EffectiveStartDate.Subtract(maxDate).Days;
                    if (taxCodeObj.EffectiveStartDate.Subtract(maxDate).Days > 1)
                    {
                        throw new ApplicationException("This taxCode's effective start date is not sequenced with previous tax code");
                    }
                }
                int newTaxCodeId = 0;
                var texcodes = _context.TaxCodes.ToList();
                if (texcodes.Count > 0) {
                    newTaxCodeId = texcodes.Max(x => x.TaxCodeId);
                }
                var userInfo = _context.Users.FirstOrDefault(x => x.UserName == _currentUserService.UserName && x.IsActive);
                Domain.Entities.TaxCodes entity = new Domain.Entities.TaxCodes
                {
                    EffectiveEndDate = taxCodeObj.EffectiveEndDate,
                    EffectiveStartDate = taxCodeObj.EffectiveStartDate,
                    TaxName = taxCodeObj.TaxName,
                    TaxTypeId = taxCodeObj.TaxTypeId,
                    TaxValue = taxCodeObj.TaxValue,
                    TaxCodeId = newTaxCodeId + 1,
                    Created = DateTime.Now,
                    CreatedBy = userInfo.UserID.ToString()
                };
                await _context.TaxCodes.AddAsync(entity, cancellationToken);
                await _context.SaveChangesAsync(cancellationToken);
                taxCodeObj.TaxCodeId = entity.TaxCodeId;
                return taxCodeObj.TaxID;
            }
        }
    }
}
