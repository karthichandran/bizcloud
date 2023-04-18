using System;
using AutoMapper;
using MediatR;
using ReProServices.Application.Common.Interfaces;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using ReProServices.Domain.Entities;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;

namespace ReProServices.Application.Customers.Queries
{
   public class GetCustomerCountQuery: IRequest<CustomerCountDto>
    {
        public class GetCustomerCountQueryHandler : IRequestHandler<GetCustomerCountQuery, CustomerCountDto>
        {
            private readonly IApplicationDbContext _context;
            private readonly IMapper _mapper;
            public GetCustomerCountQueryHandler(IApplicationDbContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }
            public async Task<CustomerCountDto> Handle(GetCustomerCountQuery request, CancellationToken cancellationToken)
            {
                var records = _context.CustomerProperty.ToList();
                var archived = records.Where(x => x.IsArchived == true).ToList().GroupBy(x => x.OwnershipID).Count();
                var unArchived = records.Where(x => x.IsArchived == null || x.IsArchived == false).ToList().GroupBy(x => x.OwnershipID).Count();

                var dto = new CustomerCountDto
                {
                    Archived = Convert.ToInt32(archived),
                    UnArchived = Convert.ToInt32(unArchived)
                };
                return  dto;
            }
        }
    }
}
