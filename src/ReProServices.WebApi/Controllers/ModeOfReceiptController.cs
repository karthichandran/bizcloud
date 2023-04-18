using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LazyCache;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ReProServices.Application.ModeOfReceipt.Queries;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860
namespace WebApi.Controllers
{
    [Authorize]
    public class ModeOfReceiptController : ApiController
    {
        private readonly IAppCache _cache;
        public ModeOfReceiptController(IAppCache cache)
        {
            _cache = cache;
        }

        [HttpGet]
        public async Task<IList<ModeOfReceiptDto>> Get()
        {

            Func<Task<IList<ModeOfReceiptDto>>> MoRGetter = () => Mediator.Send(new GetModeOfReceiptQuery());
            var MoRCache = await _cache.GetOrAddAsync("ModeOfReceipt", MoRGetter);
            return MoRCache;

        }
    }
}
