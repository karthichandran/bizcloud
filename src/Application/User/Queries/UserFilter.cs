using System;
using System.Collections.Generic;
using System.Text;

namespace ReProServices.Application.User.Queries
{
    public class UserFilter
    {
        public string UserName { get; set; }
        public string Code { get; set; }
        public bool? isActive { get; set; }
    }
}
