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
using ReProServices.Application.User;
using ReProServices.Domain;
using ReProServices.Domain.Entities;

namespace ReProServices.Application.ForGotPassword
{
   public class CheckUserIsExistQuery : IRequest<UserDto>
    {
        public ForgotPasswordDto Filter { get; set; } = new ForgotPasswordDto();
        public class CheckUserIsExistQueryHandler : IRequestHandler<CheckUserIsExistQuery, UserDto>
        {
            private readonly IApplicationDbContext _context;
            private readonly IMapper _mapper;
            public CheckUserIsExistQueryHandler(IApplicationDbContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }
            public async Task<UserDto> Handle(CheckUserIsExistQuery request, CancellationToken cancellationToken)
            {
                var filter = request.Filter;

                var entity =  _context.Users.Where(x => x.LoginName == filter.UserName && x.Email == filter.Email)
                    .ProjectTo<UserDto>(_mapper.ConfigurationProvider).FirstOrDefault();
              
                return entity;
            
            }
        }
    }
}
