using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LazyCache;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ReProServices.Application.TaxTypes.Queries;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860
namespace WebApi.Controllers
{
    [Authorize]
    public class TaxTypeController : ApiController
    {
        private readonly IAppCache _cache;
        public TaxTypeController(IAppCache cache)
        {
            _cache = cache;
        }

        [HttpGet]
        public async Task<IList<TaxTypeDto>> Get()
        {

            Func<Task<IList<TaxTypeDto>>> TaxTypeGetter = () => Mediator.Send(new GetTaxTypeQuery());
            var TaxTypeCache = await _cache.GetOrAddAsync("TaxType", TaxTypeGetter);
            return TaxTypeCache;

        }
    }
}
