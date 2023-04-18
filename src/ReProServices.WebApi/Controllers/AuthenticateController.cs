using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ReProServices.Application.User.Queries;
using ReProServices.Application.Segment.Queries;
using ReProServices.Infrastructure.Services;
using WebApi.Models;

namespace WebApi.Controllers
{
    
    public class AuthenticateController : ApiController
    {
        private readonly AuthenticationManager _authenticationMager;
       public AuthenticateController()
        {
            _authenticationMager =new AuthenticationManager();
        }

        [HttpPost("login")]
        public async Task<IActionResult> Authenticate([FromBody] AuthenticationModel user)
        {
            var dto= await Mediator.Send(new GetUserByLoginNameQuery { LoginName = user.UserName });
            if (dto == null)
                throw new ApplicationException("User does not exist");

            if (dto.LoginName.ToLower() == user.UserName.ToLower() && dto.UserPassword == user.Password && dto.IsActive)
            {
               // var _authenticationMager = new AuthenticationManager();
                var roles=await Mediator.Send(new GetRolesForClaimQuery { UserID = dto.UserID });
                return Ok( await _authenticationMager.CreateToken(Mediator, dto.UserID, dto.LoginName, roles, ipAddress()) );
            }
            else
            {
                throw new ApplicationException("Invalid user credentials");
            }
        }

       
        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken([FromBody] AuthenticateResponse refreshToken)
        {            
            var response =await _authenticationMager.CreateRefreshToken(Mediator,refreshToken.RefreshToken);

            if (response == null)
                return Unauthorized(new { message = "Invalid token" });

            //setTokenCookie(response.RefreshToken);

            return Ok(response);
        }

        private string ipAddress()
        {
            if (Request.Headers.ContainsKey("X-Forwarded-For"))
                return Request.Headers["X-Forwarded-For"];
            else
                return HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString();
        }
    }
}
