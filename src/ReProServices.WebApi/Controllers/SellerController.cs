using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LazyCache;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using ReProServices.Application.Sellers;
using ReProServices.Application.Sellers.Commands.CreateSeller;
using ReProServices.Application.Sellers.Commands.UpdateSeller;
using ReProServices.Application.Sellers.Queries.GetSellers;

namespace WebApi.Controllers
{
  
   // [Authorize(Roles="Seller")]
    public class SellerController : ApiController
    {
        private readonly IAppCache _cache;
        public SellerController(IAppCache cache)
        {
            _cache = cache;
        }

        [Authorize(Roles = "Seller_View")]
        [HttpGet]
        public  Task<IList<SellerDto>> Get([FromQuery]SellerFilter sellerFilter)
        {
            return  Mediator.Send(new GetSellersQuery() { Filter = sellerFilter});
        }

        [HttpGet("dropdown")]
        public async Task<IList<SellerDto>> GetSellerDropDown()
        {
            Func<Task<IList<SellerDto>>> SellerGetter = () => Mediator.Send(new GetSellersQuery());
            var SellerCache = await _cache.GetOrAddAsync("Seller", SellerGetter);
            return SellerCache;
        }

        [Authorize(Roles = "Seller_View")]
        [HttpGet("{id}")]
        public async Task<SellerDto> GetById(int id)
        {
            return await Mediator.Send(new GetSellersByIdQuery { SellerID = id });
        }
        [Authorize(Roles = "Seller_Create")]
        [HttpPost]
        public async Task<ActionResult<int>> Create( CreateSellerCommand command)
        {
            _cache.Remove("Seller");
            return await Mediator.Send(command);
        }
        [Authorize(Roles = "Seller_Edit")]
        [HttpPut("{id}")]
        public async Task<ActionResult> Update(long id, UpdateSellerCommand command)
        {
            if (id != command.SellerDto.SellerID)
            {
                return BadRequest();
            }

            await Mediator.Send(command);
            _cache.Remove("Seller");
            return NoContent();
        }

        //[HttpDelete("{id}")]
        //public async Task<ActionResult> Delete(long id)
        //{
        //    await Mediator.Send(new DeleteTodoItemCommand { Id = id });

        //    return NoContent();
        //}
    }
}
