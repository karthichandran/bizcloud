using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ReProServices.Application.User.Queries
{
    public static class GetUserPostQueryFilter
    {
        public static IQueryable<UserDto> PostFilterUsersBy(this IQueryable<UserDto> userDtos,
           UserFilter filter) {
            IQueryable<UserDto> userList = userDtos;
            if (!string.IsNullOrEmpty(filter.UserName)) {
                userList = userList.Where(x => x.UserName.ToLower().Contains(filter.UserName.ToLower()));
            }
            if (filter.Code != null) {
                userList = userList.Where(x => x.Code.ToLower().Contains(filter.Code.ToLower()));
            }
            if (filter.isActive != null) {
                userList = userList.Where(x => x.IsActive== filter.isActive);
            }
            return userList;
        }
    }
}
