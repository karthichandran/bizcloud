using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ReProServices.Application.CustomerBillings;
using ReProServices.Application.CustomerBillings.Commands;
using ReProServices.Application.CustomerBillings.Queries;
using WeihanLi.Npoi;

namespace WebApi.Controllers
{
    [Authorize]
    public class CustomerBillingController : ApiController
    {
        [Authorize(Roles = "CustomerBilling_View")]
        [HttpGet("getList")]
        public async Task<IList<CustomerBillingDto>> GetCustomerBillingList([FromQuery] CustomerBillingFilter customerBillingFilter)
        {
            return await Mediator.Send(new GetCustomerBillingListQuery() { Filter = customerBillingFilter });
        }
        [Authorize(Roles = "CustomerBilling_View")]
        [HttpGet("getExcel")]
        public async Task<FileResult> GetExcel([FromQuery] CustomerBillingFilter customerBillingFilter)
        {

            var resultSet = await Mediator
                .Send(new GetCustomerBillingListQuery() { Filter = customerBillingFilter });

            var settings = FluentSettings.For<CustomerBillingDto>();
            settings.HasAuthor("REpro Services");

            settings.Property(x => x.CustomerBillID)
                .HasColumnTitle("Invoice No")
                .HasColumnIndex(0);

            settings.Property(_ => _.CustomerName)
                .HasColumnTitle("Customer Name")
                .HasColumnWidth(30)
                .HasColumnIndex(1);

            settings.Property(x => x.PAN)
                .HasColumnWidth(16)
                .HasColumnIndex(2);

            settings.Property(x => x.PropertyPremises)
                .HasColumnTitle("Property Premises")
                .HasColumnWidth(30)
                .HasColumnIndex(3);

            settings.Property(x => x.UnitNo)
                .HasColumnTitle("Unit No")
                .HasColumnIndex(4);

            settings.Property(x => x.CoOwner)
                .HasColumnTitle("Is Co-Owned")
                .HasColumnWidth(16)
                .HasColumnIndex(5);
            
            settings.Property(x => x.BillDate)
                .HasColumnTitle("Invoice Date")
                .HasColumnFormatter("dd-MMM-yyy")
                .HasColumnWidth(16)
                .HasColumnIndex(6);

            settings.Property(x => x.PaymentMethodText)
                .HasColumnTitle("Payment Method")
                .HasColumnWidth(16)
                .HasColumnIndex(7);

            settings.Property(x => x.PayableByText)
                .HasColumnTitle("Payable By")
                .HasColumnWidth(16)
                .HasColumnIndex(8);
            
            settings.Property(x => x.NoOfInstallments)
                .HasColumnTitle("Installments")
                .HasColumnWidth(16)
                .HasColumnIndex(9);

            settings.Property(x => x.CostPerInstallment)
                .HasColumnTitle("Cost/Installment")
                .HasColumnWidth(18)
                .HasColumnIndex(10);

            settings.Property(x => x.Amount)
                .HasColumnTitle("Amount")
                .HasColumnIndex(11);

            settings.Property(x => x.GstRate)
                .HasColumnTitle("GST Rate")
                .HasOutputFormatter((entity, displayName) => $"{entity.GstRate}%")
                .HasColumnIndex(12);

            settings.Property(x => x.GstAmount)
                .HasColumnTitle("GST")
                .HasColumnIndex(13);

            settings.Property(x => x.TotalPayable)
                .HasColumnTitle("Total Payable")
                .HasColumnWidth(16)
                .HasColumnIndex(14);

            settings.Property(x => x.PaymentMethodID).Ignored();
            settings.Property(x => x.PayableBy).Ignored();
            settings.Property(_ => _.OwnershipID).Ignored();
            settings.Property(_ => _.CustomerID).Ignored();
            settings.Property(_ => _.PropertyID).Ignored();

            var ms = resultSet.ToExcelBytes();
            
            return File(ms, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "CustomerBilling.xls");

        }

        [HttpGet("getBaseModel/{ownershipId}")]
        public Task<CustomerBillingDto> Get(Guid ownershipId)
        {
            return Mediator.Send(new GetBaseCustomerBillingObjectByOwnershipIdQuery() { OwnershipId = ownershipId });
        }
        [Authorize(Roles = "CustomerBilling_View")]
        [HttpGet("{ownershipId}")]
        public async Task<List<CustomerBillingDto>> GetById(Guid ownershipId)
        {
            return await Mediator.Send(new GetCustomerBillingByOwnershipIdQuery { OwnershipId = ownershipId });
        }

        [HttpGet("customerBillID/{customerBillId}")]
        public async Task<CustomerBillingDto> GetByCustomerBillId(int customerBillId)
        {
            return await Mediator.Send(new GetCustomerBillByBillingIdQuery { CustomerBillID = customerBillId });
        }
        [Authorize(Roles = "CustomerBilling_Create")]
        [HttpPost]
        public async Task<CustomerBillingDto> Create(CreateCustomerBillingCommand command)
        {
            return await Mediator.Send(command);
        }
        [Authorize(Roles = "CustomerBilling_Edit")]
        [HttpPut("{id}")]
        public async Task<CustomerBillingDto> Update(long id, UpdateCustomerBillingCommand command)
        {
           return await Mediator.Send(command);
        }
        [Authorize(Roles = "CustomerBilling_Delete")]
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(long id)
        {
            await Mediator.Send(new DeleteCustomerBillingCommand { CustomerBillingID = id });

            return NoContent();
        }
    }
}
