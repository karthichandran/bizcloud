using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LazyCache;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ReProServices.Application.StatusTypes;
using ReProServices.Application.StatusTypes.Queries.GetStatusTypes;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860
namespace WebApi.Controllers
{
    [Authorize]
    public class StatusTypeController : ApiController
    {
        private readonly IAppCache _cache;
        public StatusTypeController(IAppCache cache)
        {
            _cache = cache;
        }

        [HttpGet]
        public async Task<IList<StatusTypeDto>> Get()
        {
            Func<Task<IList<StatusTypeDto>>> StatusTypeGetter = () => Mediator.Send(new GetStatusTypeQuery());
            var StatusTypeCache = await _cache.GetOrAddAsync("StatusType", StatusTypeGetter);
            return StatusTypeCache;
        }
    }
}
