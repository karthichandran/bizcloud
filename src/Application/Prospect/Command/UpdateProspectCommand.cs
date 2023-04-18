using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using ReProServices.Application.Common.Interfaces;
using ReProServices.Application.Prospect;
using ReProServices.Domain;
using ReProServices.Domain.Entities;

namespace ReProServices.Application.Prospect.Commands
{
    public class UpdateProspectCommand : IRequest<int>
    {
        public ProspectDto prospectDto { get; set; }

        public class UpdateProspectCommandHandler : IRequestHandler<UpdateProspectCommand, int>
        {
            private readonly IApplicationDbContext _context;
            public UpdateProspectCommandHandler(IApplicationDbContext context)
            {
                _context = context;
            }
            public async Task<int> Handle(UpdateProspectCommand request, CancellationToken cancellationToken)
            {
                var dto = request.prospectDto;
                var entity = _context.Prospect.FirstOrDefault(x => x.ProspectID == dto.ProspectID);

                if (entity == null)
                {
                    throw new ApplicationException("User is not found");
                }
                entity.AdressLine1 = dto.AdressLine1;
                entity.AddressLine2 = dto.AddressLine2;
                entity.AddressPremises = dto.AddressPremises;
                entity.City = dto.City;
                entity.DateOfBirth = dto.DateOfBirth;
                entity.EmailID = dto.EmailID;
                entity.IsTracesRegistered = dto.IsTracesRegistered;
                entity.DateOfBirth = dto.DateOfBirth;
                entity.MobileNo = dto.MobileNo;
                entity.Name = dto.Name;
                entity.PAN = dto.PAN;
                entity.PinCode = dto.PinCode;
                entity.StateId = dto.StateId;
                entity.TracesPassword = dto.TracesPassword;
                entity.AllowForm16B = dto.AllowForm16B;
                entity.AlternateNumber = dto.AlternateNumber;
                entity.ISD = dto.ISD;
                entity.IncomeTaxPassword = dto.IncomeTaxPassword;

                _context.Prospect.Update(entity);
                
                await _context.SaveChangesAsync(cancellationToken);
                return entity.ProspectID;
            }

        }
    }
}
