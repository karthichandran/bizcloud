using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ReProServices.Application.BankAccount;
using ReProServices.Application.BankAccount.Commands;
using ReProServices.Application.BankAccount.Queries;

namespace WebApi.Controllers
{
    public class BankAccountDetailsController : ApiController
    {
        [HttpGet("{id}")]
        public async Task<BankAccountDetailsDto> GetById(int id)
        {
            return await Mediator.Send(new GetBankAccountByIdQuery { accountId = id });
        }

        [HttpGet]
        public async Task<IList<BankAccountDetailsDto>> GetBankDetails([FromQuery] BankAccountFilter bankFilter)
        {
            return await Mediator.Send(new GetBankAccountListQuery() { Filter = bankFilter });
        }

        [HttpPost]
        public async Task<ActionResult<int>> Create(CreateBankAccountCommand command)
        {
            var result = await Mediator.Send(command);
            return result;
        }

        [HttpPut()]
        public async Task<ActionResult<int>> Update(UpdateBankAccountCommand command)
        {
            var result = await Mediator.Send(command);
            return result;
        }

        [HttpDelete("{id}" )]
        public async Task<ActionResult<bool>> Delete(int id)
        {
            return await Mediator.Send(new DeleteBankAccountCommand { AccountId=id});
        }
    }
}
