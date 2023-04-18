using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LazyCache;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ReProServices.Application.TaxCodes;
using ReProServices.Application.TaxCodes.Command.CopyTaxCodeCommand;
using ReProServices.Application.TaxCodes.Command.CreateTaxCodesCommand;
using ReProServices.Application.TaxCodes.Command.DeleteTaxCodeCommand;
using ReProServices.Application.TaxCodes.Command.UpdateTaxCodesCommand;
using ReProServices.Application.TaxCodes.Queries.GetTaxCodes;

namespace WebApi.Controllers
{
    [Authorize]
    public class TaxCodesController : ApiController
    {
        private readonly IAppCache _cache;
        public TaxCodesController(IAppCache cache)
        {
            _cache = cache;
        }
        [HttpGet]
        public async Task<IList<TaxCodesDto>> Get([FromQuery]TaxCodesFilter taxCodesFilter)
        {
            Func<Task<List<TaxCodesDto>>> TaxCodeGetter = () => Mediator.Send(new GetTaxCodesQuery() { Filter = taxCodesFilter });
            var TaxCodeCache = await _cache.GetOrAddAsync("TaxCodes", TaxCodeGetter);
            return TaxCodeCache;
        }

        [HttpGet("{id}")]
        public async Task<TaxCodesDto> GetById(int id)
        {
            return await Mediator.Send(new GetTaxCodesByIdQuery { TaxCodeId = id });
        }

        [HttpGet("TaxId/{id}")]
        public async Task<TaxCodesDto> GetByTaxId(int id)
        {
            return await Mediator.Send(new GetTaxCodeByTaxIdQuery { TaxId = id });
        }

        [HttpPost]
        public async Task<int> Create(CreateTaxCodeCommand command)
        {
            var result= await Mediator.Send(command);
            _cache.Remove("TaxCodes");
            return result;
        }

        [HttpPost("copy")]
        public async Task<ActionResult<TaxCodesDto>> Create(CopyTaxCodeCommand command)
        {
            var result = await Mediator.Send(command);
            _cache.Remove("TaxCodes");
            return result;
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<int>> Update(long id, UpdateTaxCodeCommand command)
        {
            if (id != command.TaxCodeDtoObj.TaxCodeId)
            {
                return BadRequest();
            }

            var result = await Mediator.Send(command);
            _cache.Remove("TaxCodes");
            return result;
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(long id)
        {
            await Mediator.Send(new DeleteTaxCodeCommand { TaxId=id});
            _cache.Remove("TaxCodes");
            return NoContent();
        }
    }
}
