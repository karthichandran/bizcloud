using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using ReProServices.Application.Common.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ReProServices.Application.FileCategories.GetFileCategories
{
    public class GetFileCategoryQuery : IRequest<IList<FileCategoryDto>>
    {
        public class GetFileCategoryQueryHandler : IRequestHandler<GetFileCategoryQuery, IList<FileCategoryDto>>
        {

            private readonly IApplicationDbContext _context;
            private readonly IMapper _mapper;

            public GetFileCategoryQueryHandler(IApplicationDbContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            public async Task<IList<FileCategoryDto>> Handle(GetFileCategoryQuery request, CancellationToken cancellationToken)
            {
                var vm = await _context.FileCategory
                    .ProjectTo<FileCategoryDto>(_mapper.ConfigurationProvider)
                    .ToListAsync(cancellationToken);
                return vm;

            }

        }
    }
}
