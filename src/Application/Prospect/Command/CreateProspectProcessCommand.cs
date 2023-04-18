using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ReProServices.Application.Common.Interfaces;
using ReProServices.Domain.Entities;
using ReProServices.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ReProServices.Application.Prospect.Command
{
   public class CreateProspectProcessCommand : IRequest<int>
    {
        public ProspectProcessDto ProspectProcessDto { get; set; }

        public class CreateProspectProcessCommandhandler : IRequestHandler<CreateProspectProcessCommand, int>
        {
            private readonly IApplicationDbContext _context;
            private readonly IMapper _mapper;
            private readonly ICurrentUserService _currentUserService;
            public CreateProspectProcessCommandhandler(IApplicationDbContext context, IMapper mapper, ICurrentUserService currentUserService)
            {
                _context = context;
                _mapper = mapper;
                _currentUserService = currentUserService;
            }

            public async Task<int> Handle(CreateProspectProcessCommand request, CancellationToken cancellationToken)
            {
                var userInfo = _context.Users.FirstOrDefault(x => x.UserName == _currentUserService.UserName && x.IsActive);
                var prospectProcessDto = request.ProspectProcessDto;
                var propertyDto = _context.ProspectProperty.Where(x => x.ProspectPropertyID == prospectProcessDto.ProspectPropertyID)
                   .FirstOrDefault();

                var prospectListdto =await  _context.Prospect.Where(x => x.ProspectPropertyID == prospectProcessDto.ProspectPropertyID)
                    .ToListAsync(cancellationToken);

                Guid guid = Guid.NewGuid();

                foreach (var prospectDto in prospectListdto)
                {
                    var existingCust = _context.Customer.Where(x => x.PAN.ToLower() == prospectDto.PAN.ToLower()).FirstOrDefault();
                    if (existingCust == null)
                    {
                        Customer entity = new Customer
                        {
                            AdressLine1 = prospectDto.AdressLine1,
                            AddressLine2 = prospectDto.AddressLine2,
                            AddressPremises = prospectDto.AddressPremises,
                            City = prospectDto.City,
                            DateOfBirth = prospectDto.DateOfBirth.Date,
                            EmailID = prospectDto.EmailID,
                            IsTracesRegistered = prospectDto.IsTracesRegistered,
                            MobileNo = prospectDto.MobileNo,
                            Name = prospectDto.Name,
                            PAN = prospectDto.PAN,
                            PinCode = prospectDto.PinCode,
                            StateId = prospectDto.StateId,
                            TracesPassword = prospectDto.TracesPassword,
                            AllowForm16B = prospectDto.AllowForm16B,
                            AlternateNumber = prospectDto.AlternateNumber,
                            ISD = prospectDto.ISD,
                            IncomeTaxPassword=prospectDto.IncomeTaxPassword
                            //Created = DateTime.Now,
                            //CreatedBy = userInfo.UserID.ToString()
                        };
                        await _context.Customer.AddAsync(entity, cancellationToken);
                        await _context.SaveChangesAsync(cancellationToken);
                        existingCust = entity;
                    }
                    else
                    {
                        existingCust.AdressLine1 = prospectDto.AdressLine1;
                        existingCust.AddressLine2 = prospectDto.AddressLine2;
                        existingCust.AddressPremises = prospectDto.AddressPremises;
                        existingCust.City = prospectDto.City;
                        existingCust.DateOfBirth = prospectDto.DateOfBirth.Date;
                        existingCust.EmailID = prospectDto.EmailID;
                        existingCust.IsTracesRegistered = prospectDto.IsTracesRegistered;
                        existingCust.MobileNo = prospectDto.MobileNo;
                        existingCust.Name = prospectDto.Name;
                        existingCust.PAN = prospectDto.PAN;
                        existingCust.PinCode = prospectDto.PinCode;
                        existingCust.StateId = prospectDto.StateId;
                        existingCust.TracesPassword = prospectDto.TracesPassword;
                        existingCust.AllowForm16B = prospectDto.AllowForm16B;
                        existingCust.AlternateNumber = prospectDto.AlternateNumber;
                        existingCust.ISD = prospectDto.ISD;
                        existingCust.IncomeTaxPassword = prospectDto.IncomeTaxPassword;

                        _context.Customer.Update(existingCust);
                        await _context.SaveChangesAsync(cancellationToken);
                    }

                    Domain.Entities.CustomerProperty cpEntity = new Domain.Entities.CustomerProperty
                    {
                        CustomerId = existingCust.CustomerID,
                        CustomerShare = prospectDto.Share,
                        DateOfSubmission = propertyDto.DeclarationDate,
                        GstRateID = prospectProcessDto.GstRateID,
                        IsShared = prospectListdto.Count==0?false:true,
                        PaymentMethodId = prospectProcessDto.PaymentMethodId,
                        PropertyId = propertyDto.PropertyID,                        
                        StatusTypeId = (int)EStatusType.Saved,
                        TdsCollectedBySeller = prospectProcessDto.TdsCollectedBySeller,
                        TdsRateID = prospectProcessDto.TdsRateID,
                        TotalUnitCost = prospectProcessDto.TotalUnitCost,
                        UnitNo = propertyDto.UnitNo,
                        DateOfAgreement = prospectProcessDto.DateOfAgreement?.Date,
                        OwnershipID =  guid,
                        IsArchived=false,
                        //Created = DateTime.Now,
                        //CreatedBy = userInfo.UserID.ToString()
                    };

                    await _context.CustomerProperty.AddAsync(cpEntity, cancellationToken);
                    await _context.SaveChangesAsync(cancellationToken);                   
                }

                propertyDto.OwnershipID = guid;
                _context.ProspectProperty.Update(propertyDto);
                await _context.SaveChangesAsync(cancellationToken);

                return 0;
            }
        }
    }
}
