using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ReProServices.Application.Customers.Commands.CreateCustomer;
using ReProServices.Application.Prospect;
using ReProServices.Application.Prospect.Command;
using ReProServices.Application.Prospect.Commands;
using ReProServices.Application.Prospect.Queries;

namespace WebApi.Controllers
{
   
    public class ProspectController : ApiController
    {
        [HttpPost]
        public async Task<ActionResult<int>> Create(CreateProspectAndPropertyCommand command)
        {
            var result = await Mediator.Send(command);
            return result;
        }
        [HttpPost("customer")]
        public async Task<ActionResult<int>> CreateCustomer(CreateProspectCommand command)
        {
            var result = await Mediator.Send(command);
            return result;
        }
        [HttpPost("process")]
        public async Task<ActionResult<int>> Process(CreateProspectProcessCommand command)
        {
            var result = await Mediator.Send(command);
            return result;
        }

        [HttpPut]
        public async Task<ActionResult<int>> Update(UpdateProspectCommand command)
        {
            var result = await Mediator.Send(command);
            return result;
        }

        [HttpPut("updateShares")]
        public async Task<ActionResult<int>> UpdateShares(UpdateProspectSharesCommand command)
        {
            var result = await Mediator.Send(command);
            return result;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ProspectDto>> Get(int id)
        {
            var result = await Mediator.Send(new GetProspectByIdQuery{ prospectID=id});
            return result;
        }
        [HttpGet("list")]
        public async Task<List<ProspectListVM>> GetList([FromQuery] ProspectFilter prospectFilter)
        {
            var result = await Mediator.Send(new GetProspectListQuery { Filter= prospectFilter });
            return result;
        }

        [HttpGet("prospectAndProperty/{id}")]
        public async Task<ProspectVm> GetProspectAndProperty(int id)
        {
            var result = await Mediator.Send(new GetProspectWithPropertyByIdQuery { prospectPropertyID = id }) ;
            return result;
        }

        [HttpDelete("{id}")]
        public async Task<Unit> Delete(int id) {
            var result = await Mediator.Send(new DeleteProspectAndPropertyCommand { prospectPropertyID = id });
            return result;
        }
    }
}
