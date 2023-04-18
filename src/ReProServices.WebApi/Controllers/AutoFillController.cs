using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ReProServices.Application.AutoFill;
using ReProServices.Application.BankAccount;
using ReProServices.Application.BankAccount.Queries;

namespace WebApi.Controllers
{
    public class AutoFillController :  ApiController
    {
        [HttpGet("{id}")]
        public async Task<AutoFillDto> GetById(int id)
        {
            return await Mediator.Send(new GetForm26BByTransactionIdQuery { ClientPaymentTransactionID = id });
        }

        [HttpGet("UserDetail")]
        public async Task<BankAccountDetailsDto> GetBankDetails()
        {
            return await Mediator.Send(new GetBankAccountDetailsQuery());
        }

        [HttpGet("AccountList")]
        public async Task<IList<BankAccountDetailsDto>> GetAccountList()
        {
            return await Mediator.Send(new GetBankAccountListQuery() { Filter = new BankAccountFilter() { UserName = "" } });
        }
    }
}
