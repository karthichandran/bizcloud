using System;
using System.Collections.Generic;
using System.Text;

namespace ReProServices.Application.Receipts
{
    public class CoOwnersDto
    {
        public string CustomerName { get; set;}
        public string Email { get; set; }
        public bool isprimaryOwner { get; set; }
    }
}
