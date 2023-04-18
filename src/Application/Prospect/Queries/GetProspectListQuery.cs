using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using ReProServices.Application.Common.Interfaces;
using ReProServices.Application.Roles;
using ReProServices.Domain;
using ReProServices.Domain.Entities;
namespace ReProServices.Application.Prospect.Queries
{
   public class GetProspectListQuery : IRequest<List<ProspectListVM>>
    {
        public ProspectFilter Filter { get; set; } = new ProspectFilter();
        public class GetProspectLIstQueryHandler : IRequestHandler<GetProspectListQuery, List<ProspectListVM>>
        {
            private readonly IApplicationDbContext _context;
            private readonly IMapper _mapper;
            public GetProspectLIstQueryHandler(IApplicationDbContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }
            public async Task<List<ProspectListVM>> Handle(GetProspectListQuery request, CancellationToken cancellationToken)
            {
                var prospectPropertyFiltered = _context.ProspectProperty.Where(x => x.OwnershipID == null);
                var filter = request.Filter;
                var dtoQuery =  (from p in _context.Prospect
                                 join pp in prospectPropertyFiltered on p.ProspectPropertyID equals pp.ProspectPropertyID   
                                 join pt in _context.Property on pp.PropertyID equals pt.PropertyID
                                 where pp.OwnershipID== null &&
                                 filter.propertyID>0?pp.PropertyID== filter.propertyID:true  &&
                                 !string.IsNullOrEmpty(filter.PAN)?p.PAN.ToLower()==filter.PAN.ToLower():true
                                 select new 
                                 {
                                     prospectPropertyID =p.ProspectPropertyID,
                                     name=p.Name,
                                     premises=pt.AddressPremises,
                                     propertyId=pt.PropertyID,
                                     unitNo=pp.UnitNo,
                                     declarationDate=pp.DeclarationDate,
                                     share=p.Share
                                 }).ToList();

                var list = dtoQuery.GroupBy(x => x.prospectPropertyID).Select(x => new ProspectListVM
                {
                    propertyID = x.First().propertyId,
                    ProspectPropertyID = x.First().prospectPropertyID,
                    name = string.Join(",", x.Select(g =>  g.name + "/" + g.share )),
                    unitNo = x.First().unitNo,
                    DeclarationDate = x.First().declarationDate,
                    Premises = x.First().premises
                }).AsQueryable().PreFilter(filter).ToList();

                return list;
            }
        }
    }
}
