using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LazyCache;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ReProServices.Application.Property;
using ReProServices.Application.Property.Commands.CreateProperty;
using ReProServices.Application.Property.Commands.UpdateProperty;
using ReProServices.Application.Property.Queries;

namespace WebApi.Controllers
{
   // [Authorize]
    public class PropertyController : ApiController
    {
        private readonly IAppCache _cache;
        public PropertyController(IAppCache cache)
        {
            _cache = cache;
        }
        [Authorize(Roles = "Property_View")]
        [HttpGet]
        public async Task<IList<PropertyDto>> Get([FromQuery] PropertyFilter propertyFilter )
        {
            return await Mediator.Send(new GetPropertiesQuery() { Filter = propertyFilter });
        }

        [HttpGet("dropdown")]
        public async Task<IList<PropertyDto>> GetDropDownList()
        {
            Func<Task<IList<PropertyDto>>> PropertyGetter = () => Mediator.Send(new GetPropertiesQuery());
            var PropertyCache = await _cache.GetOrAddAsync("Property", PropertyGetter);
            return PropertyCache;
        }
       
        [HttpGet("{id}")]
        public async Task<PropertyVM> GetById(int id)
        {
            return await Mediator.Send(new GetPropertyByIdQuery { PropertyID = id });
        }
        [Authorize(Roles = "Property_Create")]
        [HttpPost]
        public async Task<ActionResult<PropertyVM>> Create(CreatePropertyCommand command)
        {
            var result = await Mediator.Send(command);
            _cache.Remove("Property");
            return result;
        }
        [Authorize(Roles = "Property_Edit")]
        [HttpPut("{id}")]
        public async Task<ActionResult> Update(long id, UpdatePropertyCommand command)
        {
            if (id != command.PropertyVM.propertyDto.PropertyID)
            {
                return BadRequest();
            }

            await Mediator.Send(command);
            _cache.Remove("Property");
            return NoContent();
        }
      
    }
}
