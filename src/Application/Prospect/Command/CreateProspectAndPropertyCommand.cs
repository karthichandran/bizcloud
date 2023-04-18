using MediatR;
using ReProServices.Application.Common.Interfaces;
using ReProServices.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ReProServices.Application.Prospect.Command
{
    public class CreateProspectAndPropertyCommand : IRequest<int>
    {
        public ProspectVm prospectVm { get; set; }

        public class CreateProspectAndPropertyCommandhandler : IRequestHandler<CreateProspectAndPropertyCommand, int>
        {
            private readonly IApplicationDbContext _context;
            public CreateProspectAndPropertyCommandhandler(IApplicationDbContext context)
            {
                _context = context;
            }

            public async Task<int> Handle(CreateProspectAndPropertyCommand request, CancellationToken cancellationToken)
            {
                try
                {
                    var prospectPropertyDto = request.prospectVm.ProspectPropertyDto;
                    var existingPros = _context.ProspectProperty.Where(x => x.PropertyID == prospectPropertyDto.PropertyID && x.UnitNo == prospectPropertyDto.UnitNo).FirstOrDefault();

                    if (existingPros != null)
                        throw new ApplicationException("Property detail is already exist");

                    var prospectPropertyEntity = new ProspectProperty
                    {
                        PropertyID = prospectPropertyDto.PropertyID,
                        DeclarationDate = prospectPropertyDto.DeclarationDate,
                        UnitNo = prospectPropertyDto.UnitNo
                    };
                    await _context.ProspectProperty.AddAsync(prospectPropertyEntity, cancellationToken);
                    await _context.SaveChangesAsync(cancellationToken);

                    foreach (var dto in request.prospectVm.ProspectDto)
                    {
                        Domain.Entities.Prospect entity = new Domain.Entities.Prospect
                        {
                            ProspectPropertyID = prospectPropertyEntity.ProspectPropertyID,
                            AddressPremises = dto.AddressPremises,
                            AdressLine1 = dto.AdressLine1,
                            AddressLine2 = dto.AddressLine2,
                            City = dto.City,
                            DateOfBirth = dto.DateOfBirth.Date,
                            EmailID = dto.EmailID,
                            IsTracesRegistered = dto.IsTracesRegistered,
                            MobileNo = dto.MobileNo,
                            Name = dto.Name,
                            PAN = dto.PAN,
                            PinCode = dto.PinCode,
                            StateId = dto.StateId,
                            TracesPassword = dto.TracesPassword,
                            AllowForm16B = dto.AllowForm16B,
                            AlternateNumber = dto.AlternateNumber,
                            ISD = dto.ISD,
                            Share = dto.Share,
                            PanBlobID = dto.PanBlobId,
                            IncomeTaxPassword   =dto.IncomeTaxPassword
                        };

                        await _context.Prospect.AddAsync(entity, cancellationToken);
                    }
                    await _context.SaveChangesAsync(cancellationToken);

                    return prospectPropertyEntity.ProspectPropertyID;
                }
                catch (Exception ex) {
                    throw ex;
                }
            }
        }
    }
}
