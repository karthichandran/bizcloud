using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ReProServices.Application.SellerProperties;
using ReProServices.Application.SellerProperties.Queries;

namespace WebApi.Controllers
{
    [Authorize]
    public class SellerPropertyController : ApiController
    {
        [HttpGet]
        public  Task<IList<SellerPropertyVM>> Get([FromQuery]SellerPropertyFilter spFilter)
        {
            return  Mediator.Send(new GetSellerPropertyQuery() { Filter = spFilter });
        }

        //[HttpGet("{id}")]
        //public async Task<SellerDto> GetById(int id)
        //{
        //    return await Mediator.Send(new GetSellersByIdQuery { SellerID = id });
        //}

        //[HttpPost]
        //public async Task<ActionResult<int>> Create( CreateSellerCommand command)
        //{
        //    return await Mediator.Send(command);
        //}

        //[HttpPut("{id}")]
        //public async Task<ActionResult> Update(long id, UpdateSellerCommand command)
        //{
        //    if (id != command.sellerDto.SellerID)
        //    {
        //        return BadRequest();
        //    }

        //    await Mediator.Send(command);

        //    return NoContent();
        //}

        //[HttpDelete("{id}")]
        //public async Task<ActionResult> Delete(long id)
        //{
        //    await Mediator.Send(new DeleteTodoItemCommand { Id = id });

        //    return NoContent();
        //}
    }
}
