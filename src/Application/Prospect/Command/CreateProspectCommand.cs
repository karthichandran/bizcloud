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
    public class CreateProspectCommand : IRequest<int>
    {
        public ProspectDto prospectDto { get; set; }

        public class CreateProspectCommandhandler : IRequestHandler<CreateProspectCommand, int>
        {
            private readonly IApplicationDbContext _context;
            public CreateProspectCommandhandler(IApplicationDbContext context)
            {
                _context = context;
            }

            public async Task<int> Handle(CreateProspectCommand request, CancellationToken cancellationToken)
            {
                var dto = request.prospectDto;
                    Domain.Entities.Prospect entity = new Domain.Entities.Prospect
                    {
                        ProspectPropertyID = dto.ProspectPropertyID,
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
                        PinCode = dto.PinCode.Trim(),
                        StateId = dto.StateId,
                        TracesPassword = dto.TracesPassword,
                        AllowForm16B = dto.AllowForm16B,
                        AlternateNumber = dto.AlternateNumber,
                        ISD = dto.ISD,
                        Share = dto.Share,
                        PanBlobID = dto.PanBlobId,
                        IncomeTaxPassword=dto.IncomeTaxPassword
                    };

                    await _context.Prospect.AddAsync(entity, cancellationToken);               
                await _context.SaveChangesAsync(cancellationToken);

                return entity.ProspectID;
            }
        }
    }
}
