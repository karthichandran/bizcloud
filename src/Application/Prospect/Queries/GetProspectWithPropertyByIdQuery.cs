using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ReProServices.Application.Common.Interfaces;
using ReProServices.Application.Roles;
using ReProServices.Domain;
using ReProServices.Domain.Entities;

namespace ReProServices.Application.Prospect.Queries
{
    public class GetProspectWithPropertyByIdQuery : IRequest<ProspectVm>
    {
        public int prospectPropertyID { get; set; }

        public class GetProspectWithPropertyByIdQueryHandler : IRequestHandler<GetProspectWithPropertyByIdQuery, ProspectVm>
        {
            private readonly IApplicationDbContext _context;
            private readonly IMapper _mapper;
            public GetProspectWithPropertyByIdQueryHandler(IApplicationDbContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }
            public async Task<ProspectVm> Handle(GetProspectWithPropertyByIdQuery request, CancellationToken cancellationToken)
            {
                var propertyDto = _context.ProspectProperty.Where(x => x.ProspectPropertyID == request.prospectPropertyID)
                    .ProjectTo<ProspectPropertyDto>(_mapper.ConfigurationProvider).FirstOrDefault();

                var prospectListdto = await _context.Prospect.Where(x => x.ProspectPropertyID == request.prospectPropertyID)
                    .ProjectTo<ProspectDto>(_mapper.ConfigurationProvider).ToListAsync(cancellationToken);

                var vm = new ProspectVm
                {
                    ProspectPropertyDto = propertyDto,
                    ProspectDto = prospectListdto
                };
                return vm;
            }
        }
    }
}
