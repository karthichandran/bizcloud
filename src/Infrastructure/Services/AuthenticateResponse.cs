using System;
using System.Collections.Generic;
using System.Text;

namespace ReProServices.Infrastructure.Services
{
    public class AuthenticateResponse
    {    
        public string Token { get; set; }
        public string RefreshToken { get; set; }

    }
}
