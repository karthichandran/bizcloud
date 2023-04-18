using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LazyCache;
using Microsoft.AspNetCore.Mvc;
using ReProServices.Application.RemittanceStatusList;
using ReProServices.Application.RemittanceStatusList.Queries;

namespace WebApi.Controllers
{
    //[Authorize]
    public class RemittanceStatusController : ApiController

    {
        private readonly IAppCache _cache;
        public RemittanceStatusController(IAppCache cache)
        {
            _cache = cache;
        }

        [HttpGet]
        public async Task<IList<RemittanceStatusDto>> Get()
        {
            Func<Task<IList<RemittanceStatusDto>>> RemStatusGetter = () => Mediator.Send(new GetRemittanceStatusQuery());
            var RemStatusCache = await _cache.GetOrAddAsync("RemittanceStatus", RemStatusGetter);
            return RemStatusCache;

           
        }
    }
}
