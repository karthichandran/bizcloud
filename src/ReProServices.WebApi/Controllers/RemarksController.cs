using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LazyCache;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ReProServices.Application.Remarks;
using ReProServices.Application.Remarks.Queries;

namespace WebApi.Controllers
{
    [Authorize]
    public class RemarksController : ApiController
    {
        private readonly IAppCache _cache;
        public RemarksController(IAppCache cache)
        {
            _cache = cache;
        }

        [HttpGet]
        public async Task<IList<RemarkDto>> Get()
        {
            Func<Task<IList<RemarkDto>>> RemarksGetter = () => Mediator.Send(new GetRemarksQuery());
            var remarksCache = await _cache.GetOrAddAsync("Remarks", RemarksGetter);
            return remarksCache;

        }
    }
}
