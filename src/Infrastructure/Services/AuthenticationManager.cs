using MediatR;
using Microsoft.IdentityModel.Tokens;
using ReProServices.Application.User;
using ReProServices.Application.User.Command;
using ReProServices.Application.User.Queries;
using ReProServices.Application.Segment.Queries;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
#pragma warning disable 1998

namespace ReProServices.Infrastructure.Services
{
   public class AuthenticationManager
    {        
        public async Task<AuthenticateResponse> CreateToken(IMediator mediator, int userId,string userName,List<string> roles,string cratedByIp)
        {
            var signingCredentials = GetSigningCredentials();
            var claims = await GetClaims(userName, roles);
            var tokenOptions = GenerateTokenOptions(signingCredentials, claims);
            var refreshToken = generateRefreshToken();
            var expires = DateTime.Now.AddMinutes(7);
            var userSession = new UserSessionDto
            {
                UserID = userId,
                CreatedByIp = cratedByIp,
                Expires = expires,
                Created = DateTime.Now,
                RefreshToken = refreshToken
            };
            var result = await mediator.Send(new CreateUserSessionCommand { UserSessionDto = userSession });
            var response = new AuthenticateResponse {             
            Token= new JwtSecurityTokenHandler().WriteToken(tokenOptions),
            RefreshToken= refreshToken
            };
            return response;
        }

        public async Task<AuthenticateResponse> CreateRefreshToken(IMediator mediator,  string token)
        {
            var userSession = await mediator.Send(new GetUserSessionQuery { Token = token });
            var user = await mediator.Send(new GetUserByIdQuery { UserID = userSession.UserID });
            var roles = await mediator.Send(new GetRolesForClaimQuery { UserID = userSession.UserID });

            var signingCredentials = GetSigningCredentials();
            var claims = await GetClaims(user.userDto.UserName, roles);
            var tokenOptions = GenerateTokenOptions(signingCredentials, claims);
            var refreshToken = generateRefreshToken();
            var expires = DateTime.Now.AddMinutes(7);
            var newUserSession = new UserSessionDto
            {               
                Expires = expires,               
                RefreshToken = refreshToken
            };
            var result = await mediator.Send(new UpdateUserSessonCommand { UserSessionDto = newUserSession,Token=token });
            var response = new AuthenticateResponse
            {
                Token = new JwtSecurityTokenHandler().WriteToken(tokenOptions),
                RefreshToken = refreshToken
            };
            return response;
        }

        private SigningCredentials GetSigningCredentials()
        {
            var key = Encoding.UTF8.GetBytes("ReproServicesKey");
            var secret = new SymmetricSecurityKey(key);
            return new SigningCredentials(secret, SecurityAlgorithms.HmacSha256);
        }

        private async Task<List<Claim>> GetClaims(string userName, List<string> roles)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, userName)
            };
            //claims.Add(new Claim( ClaimTypes.Role, "admin"));
            //var roles = await _userManager.GetRolesAsync(_user);
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            return claims;
        }

        private JwtSecurityToken GenerateTokenOptions(SigningCredentials signingCredentials, List<Claim> claims)
        {
           
            var tokenOptions = new JwtSecurityToken
            (
                issuer: "ReproService",
                audience: "https://localhost:4200",
                claims: claims,
                expires: DateTime.Now.AddMinutes(Convert.ToDouble("5")),
                signingCredentials: signingCredentials
            );

            return tokenOptions;
        }       

        private string generateRefreshToken()
        {
            using (var rngCryptoServiceProvider = new RNGCryptoServiceProvider())
            {
                var randomBytes = new byte[64];
                rngCryptoServiceProvider.GetBytes(randomBytes);
                return Convert.ToBase64String(randomBytes);
                 
            }
        }

    }
}
