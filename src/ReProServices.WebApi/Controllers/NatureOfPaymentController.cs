using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ReProServices.Application.NatureOfPayments.Queries;
using ReProServices.Application.NatureOfPayments.Queries.GetNatureOfPayments;
using LazyCache;
using Microsoft.AspNetCore.Authorization;


// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApi.Controllers
{
    [Authorize]
    public class NatureOfPaymentController : ApiController

    {
        private readonly IAppCache _cache;
        public NatureOfPaymentController(IAppCache cache)
        {
            _cache = cache;
        }
        [HttpGet]
        public async Task<IList<NatureOfPaymentDto>> Get()
        {
            Func<Task<IList<NatureOfPaymentDto>>> NoPGetter = () => Mediator.Send(new GetNatureOfPaymentsQuery());
            var NoPCache = await _cache.GetOrAddAsync("NatureOfPayment", NoPGetter);
            return NoPCache;
        }
    }
}
