using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ReProServices.Application.Common.Interfaces;

namespace ReProServices.Application.SellerProperties.Queries
{
    public class GetSellerPropertyQuery : IRequest<IList<SellerPropertyVM>>
    {
        public SellerPropertyFilter Filter { get; set; } = new SellerPropertyFilter();
        public class GetSellerPropertyQueryHandler : IRequestHandler<GetSellerPropertyQuery, IList<SellerPropertyVM>>
        {

            private readonly IApplicationDbContext _context;
            private readonly IMapper _mapper;

            public GetSellerPropertyQueryHandler(IApplicationDbContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            public async Task<IList<SellerPropertyVM>> Handle(GetSellerPropertyQuery request, CancellationToken cancellationToken)
            {
               var vm = await  _context.ViewSellerPropertyBasic 
                                .FilterSellerPropertiesBy(request.Filter)
                                .ToListAsync(cancellationToken);

               var groupedPropertyList = vm.GroupBy(u => u.PropertyID)
                   .Select(grp => grp.ToList());

                IList<SellerPropertyVM> sellerVms = new List<SellerPropertyVM>();

                foreach (var grp in groupedPropertyList)
                {
                    sellerVms.Add(new SellerPropertyVM
                    {
                        PropertyId = grp[0].PropertyID,
                        AddressPremises = grp[0].PropertyPremises,
                        SellerNames  = string.Join(",", grp.Select(g => g.SellerName)),
                        PanNumbers = string.Join(",", grp.Select(g => g.SellerPAN)),
                        PropertyShortName = grp[0].PropertyShortName
                    });
                }

                var spVm = (from sp in sellerVms
                           join p in _context.Property on sp.PropertyId equals p.PropertyID
                           select new SellerPropertyVM
                           {
                               PropertyId = sp.PropertyId,
                               AddressPremises = sp.AddressPremises,
                               SellerNames = sp.SellerNames,
                               PanNumbers = sp.PanNumbers,
                               PropertyShortName = sp.PropertyShortName,
                               IsActive = p.IsActive
                           }).ToList();
                //return sellerVms;
                return spVm;
            }

        }
    }
}
