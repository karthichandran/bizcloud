using ReProServices.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace ReProServices.Application.User
{
    public class UserVM
    {
        public UserDto userDto { get; set; }
        public ICollection<UserRolesDto> userRolesDto { get; set; }
    }
}
