using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using ReProServices.Application.Common.Interfaces;

namespace WebApi.Services
{
    public class CurrentUserService : ICurrentUserService
    {
        public CurrentUserService(IHttpContextAccessor httpContextAccessor)
        {
            UserId = httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
            UserName = httpContextAccessor.HttpContext?.User?.Identity.Name;
        }

        public string UserId { get; }

        public string UserName { get; set; }
    }
}
