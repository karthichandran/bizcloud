using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using LazyCache;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using ReProServices.Application.Sellers;
using ReProServices.Application.Sellers.Commands.CreateSeller;
using ReProServices.Application.Sellers.Commands.UpdateSeller;
using ReProServices.Application.Sellers.Queries.GetSellers;
using ReProServices.Application.Remittance_Remark;
using ReProServices.Application.Remittance_Remark.Queries;
using ReProServices.Application.Remittance_Remark.Commands;

namespace WebApi.Controllers
{
    public class RemittanceRemarkController : ApiController
    {
        private IConfiguration _configuration;
        public RemittanceRemarkController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

      
        [HttpGet]
        public async Task<List<RemittanceRemarkDto>> Get([FromQuery] RemarkFilter filter)
        {
            return await Mediator.Send(new GetRemittanceRemarkQuery() { filter= filter });
        }
        [HttpGet("{id}")]
        public async Task<RemittanceRemarkDto> Get(int id)
        {
            return await Mediator.Send(new GetRemittanceRemarkByIdQuery() { Id = id });
        }

        [HttpPost]
        public async Task<bool> Post(CreateRemittanceRemarkCommand command)
        {
            return await Mediator.Send(command);
        }

        [HttpPut]
        public async Task<bool> Put(UpdateRemittanceRemarkCommand command)
        {
            return await Mediator.Send(command);
        }
                     
        [HttpDelete("{id}")]
        public async Task<bool> Delete(int id)
        {
            return await Mediator.Send( new DeleteRemittanceRemarkCommand { remarkId=id });
        }

        [HttpPut("remittance/{clientPayTransId}/{remarkId}")]
        public async Task<bool> SaveRemittanceRemark(int clientPayTransId,int remarkId)
        {
            return await Mediator.Send(new SaveTransRemittanceRemarkCommand(){ ClientPaymentTransactionId= clientPayTransId ,RemarkId=remarkId});
        }

        [HttpPut("traces/{clientPayTransId}/{remarkId}")]
        public async Task<bool> SaveTracesRemark(int clientPayTransId, int remarkId)
        {
            return await Mediator.Send(new SaveTransTracesRemarkCommand() { ClientPaymentTransactionId = clientPayTransId, RemarkId = remarkId });
        }
    }
}
