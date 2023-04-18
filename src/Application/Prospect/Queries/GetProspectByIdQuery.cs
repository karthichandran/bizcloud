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
   public class GetProspectByIdQuery : IRequest<ProspectDto>
    {
        public int prospectID { get; set; }

        public class GetProspectByIdQueryHandler : IRequestHandler<GetProspectByIdQuery, ProspectDto>
        {
            private readonly IApplicationDbContext _context;
            private readonly IMapper _mapper;
            public GetProspectByIdQueryHandler(IApplicationDbContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }
            public async Task<ProspectDto> Handle(GetProspectByIdQuery request, CancellationToken cancellationToken)
            {
                var dto = await _context.Prospect.Where(x => x.ProspectID == request.prospectID)
                    .ProjectTo<ProspectDto>(_mapper.ConfigurationProvider)
                        .FirstOrDefaultAsync(cancellationToken);
                
                return dto;
            }
        }
    }
}
