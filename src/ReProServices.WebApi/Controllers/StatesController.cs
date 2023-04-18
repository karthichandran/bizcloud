using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LazyCache;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ReProServices.Application.States.Queries.GetStates;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApi.Controllers
{
   
    public class StatesController : ApiController
    {
        private readonly IAppCache _cache;
        public StatesController(IAppCache cache)
        {
            _cache = cache;
        }

        [HttpGet]
        public async Task<IList<StateDto>> Get()
        {
            Func<Task<IList<StateDto>>> statesGetter = () =>   Mediator.Send(new GetStatesQuery());
            var statesWithCaching = await _cache.GetOrAddAsync("States", statesGetter);
            return statesWithCaching;
        }
    }
}
