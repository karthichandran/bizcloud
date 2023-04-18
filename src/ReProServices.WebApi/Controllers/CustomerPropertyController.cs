using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ReProServices.Application.CustomerProperty.Commands.CreateCustomerProperty;
using ReProServices.Application.CustomerProperty.Commands.UpdateCustomerProeprtyStatus;
using ReProServices.Application.CustomerProperty.Commands.UpdateCustomerProperty;
using ReProServices.Application.Customers;

namespace WebApi.Controllers
{
    [Authorize]
    public class CustomerPropertyController : ApiController
    {
        [HttpPost]
        public async Task<ActionResult<CustomerVM>> Create(CreateCustomerPropertyCommand command)
        {
            var result = await Mediator.Send(command);
            return result;
        }

        [HttpPut()]
        public async Task<ActionResult<CustomerVM>> Update(UpdateCustomerPropertyCommand command)
        {
            var result = await Mediator.Send(command);
            return result;
        }

        //[HttpPut("{ownershipID}/{StatusTypeID}/{Remarks}")]
        //public async Task<Unit> UpdateStatus(Guid ownershipID, int statusTypeID, string remarks)
        //{
        //    var cpObj = new CustPropStatusObj() {
        //        OwnershipID = ownershipID,
        //        StatusTypeID = statusTypeID,
        //        Remarks = remarks
        //    };

        //    var result = await Mediator.Send(new UpdateCustomerPropertyStatusCommand { CustPropStatusObj = cpObj });
        //    return result;
        //}

        [HttpPut("Status")]
        public async Task<Unit> UpdateStatus(UpdateCustomerPropertyStatusCommand command)
        {
            var result = await Mediator.Send(command);
            return result;
        }
    }
}
