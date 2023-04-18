using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using ExcelDataReader;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ReProServices.Application.Customers;
using ReProServices.Application.Customers.Commands.CreateCustomer;
using ReProServices.Application.Customers.Commands.DeleteCustomer;
using ReProServices.Application.Customers.Commands.UpdateCustomer;
using ReProServices.Application.Customers.Queries;
using ReProServices.Domain;
using ReProServices.Domain.Entities;
using WeihanLi.Npoi;
using ReProServices.Infrastructure.Smtp;
using System.Drawing;
using System.Drawing.Imaging;
using System.Net.Mail;
using Microsoft.Extensions.Configuration;
using ReProServices.Application.Property.Queries;

namespace WebApi.Controllers
{
    [Authorize]
    public class CustomerController : ApiController
    {
        private IConfiguration _configuration;
        public CustomerController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [Authorize(Roles = "Client_View")]
        [HttpGet]
        public async Task<CustomerVM> Get([FromQuery]CustomerDetailsFilter customerDetailsFilter)
        {
            return await Mediator.Send(new GetCustomersQuery() { Filter = customerDetailsFilter});
        }
        [Authorize(Roles = "Client_View")]
        [HttpGet("getCustomerCount")]
        public async Task<CustomerCountDto> GetCustomerCount()
        {
            return await Mediator.Send(new GetCustomerCountQuery() {  });
        }
        [Authorize(Roles = "Client_View")]
        [HttpGet("getExcel")]
        public async Task<FileResult> GetExcel([FromQuery]CustomerDetailsFilter customerDetailsFilter)
        {
            var result = await Mediator.Send(new GetCustomersQuery() { Filter = customerDetailsFilter });
            var resultSet = result.customersView;

            var settings = FluentSettings.For<ViewCustomerPropertyBasic>();
            settings.HasAuthor("REpro Services");
            
            settings.Property(_ => _.CustomerName)
                .HasColumnTitle("Customer Name")
                .HasColumnWidth(50)
                .HasColumnIndex(0);

            settings.Property(x => x.PAN)
                .HasColumnWidth(16)
                .HasColumnIndex(1);

            settings.Property(x => x.PropertyPremises)
                .HasColumnTitle("Property Premises")
                .HasColumnWidth(30)
                .HasColumnIndex(2);

            settings.Property(x => x.UnitNo)
                .HasColumnTitle("Unit No")
                .HasColumnWidth(18)
                .HasColumnIndex(3);

            settings.Property(x => x.TotalUnitCost)
                .HasColumnTitle("Unit Cost")
                .HasColumnWidth(18)
                .HasColumnIndex(4);

            settings.Property(x => x.DateOfAgreement)
                .HasColumnTitle("Date of Agreement")
                .HasColumnFormatter("dd-MMM-yyy")
                .HasColumnWidth(18)
                .HasColumnIndex(5);

            settings.Property(x => x.DateOfSubmission)
                .HasColumnTitle("Date of Submission")
                .HasColumnFormatter("dd-MMM-yyy")
                .HasColumnWidth(18)
                .HasColumnIndex(6);

            settings.Property(x => x.Remarks)
                .HasColumnTitle("Remarks")
                .HasColumnWidth(60)
                .HasColumnIndex(7);

            settings.Property(x => x.TracesPassword)
              .HasColumnTitle("Traces Password")
              .HasColumnWidth(60)
              .HasColumnIndex(8);

            settings.Property(x => x.CustomerAlias)
             .HasColumnTitle("Alias")
             .HasColumnWidth(60)
             .HasColumnIndex(9);

            settings.Property(x => x.StampDuty)
            .HasColumnTitle("Stamp Duty")
            .HasColumnWidth(60)
            .HasColumnIndex(10);

            settings.Property(x => x.IncomeTaxPassword)
         .HasColumnTitle("IT Password")
         .HasColumnWidth(60)
         .HasColumnIndex(11);


            settings.Property(_ => _.OwnershipID).Ignored();
            settings.Property(_ => _.CustomerID).Ignored();
            settings.Property(_ => _.PropertyID).Ignored();
            settings.Property(_ => _.CustomerPropertyID).Ignored();
            settings.Property(_ => _.OwnershipID).Ignored();
            settings.Property(_ => _.PaymentMethodId).Ignored();
            settings.Property(_ => _.StatusTypeID).Ignored();

            var ms = resultSet.ToExcelBytes();

            return File(ms, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "CustomerDetails.xls");

        }
        [Authorize(Roles = "Client_View")]
        [HttpGet("{id}")]
        public async Task<CustomerVM> GetById(Guid id)
        {
            return await Mediator.Send(new GetCustomerByIDQuery { OwnershipId = id });
        }

        [HttpGet("PAN/{pan}")]
        public async Task<CustomerDto> GetByPAN(string pan)
        {
            return await Mediator.Send(new GetCustomerByPANQuery { PAN = pan });
        }
        [Authorize(Roles = "Client_Create")]
        [HttpPost]
        public async Task<ActionResult<CustomerVM>> Create(CreateCustomerCommand command)
        {
            var result = await Mediator.Send(command);
            return result;
        }
        [Authorize(Roles = "Client_Edit")]
        [HttpPut()]
        public async Task<ActionResult<CustomerVM>> Update(UpdateCustomerCommand command)
        {
            var result = await Mediator.Send(command);
            return result;
        }

        [HttpDelete("{id}/{ownershipid}")]
        public async Task<Unit> Delete(int id,Guid ownershipId) {
            return await Mediator.Send(new DeleteCustomerCommand { CustomerID = id ,OwnershipID=ownershipId});
        }

        //[HttpGet("groupMail/{id}")]
        //public async Task<bool> SendGroupMail(Guid id)
        //{

        //    var dto = await Mediator.Send(new GetCustomerByIDQuery { OwnershipId = id });
        //    var projectId = dto.customers.First().CustomerProperty.First().PropertyId;
        //    var unitNo = dto.customers.First().CustomerProperty.First().UnitNo;
        //    var projObj = await Mediator.Send(new GetPropertyByIdQuery { PropertyID = projectId });
        //    var project = projObj.propertyDto.AddressPremises;
        //    var filePath = @Directory.GetCurrentDirectory() + "\\Resources\\logo.png";

        //    Bitmap b = new Bitmap(filePath);
        //    MemoryStream ms = new MemoryStream();
        //    b.Save(ms, ImageFormat.Png);
        //    ms.Position = 0;
        //    var logoResource = new LinkedResource(ms, "image/png") { ContentId = "added-image-id" };
        //    var subject = "Urgent !! Income tax new portal 2.0 - Impact on TDS payments U/s. 194IA on your behalf –" + project + " - " + unitNo;

        //    var template = "";
        //    var toList = "";
        //    foreach (var cus in dto.customers)
        //    {
        //        if (!string.IsNullOrEmpty(cus.EmailID))
        //            toList += cus.EmailID + ",";
        //        template += "<tr><td class='cell'>" + cus.Name + "</td><td class='cell'>" + cus.PAN + "</td><td class='cell'>" + cus.IncomeTaxPassword + "</td></tr>";
        //    }
        //    var table = "<table style='width:100%; border-collapse: collapse;'><tr><td class='cell-header'>Name of the Owner </td><td class='cell-header'> PAN</td><td class='cell-header'>Income Tax Login Password </td></tr>" + template + "</table>";

        //    if (!string.IsNullOrEmpty(toList))
        //        toList = toList.Substring(0, toList.Length - 1);

        //    var emilaModel = new EmailModel()
        //    {
        //        //To="karthi@leansys.in",
        //        To = toList,
        //        From= "support@reproservices.in",
        //        Subject = subject,
        //        IsBodyHtml = true
        //    };


        //    emilaModel.Message = @"<html><style> .cell-header{text-align: center;width: 33%;height: 35px;display: inline-block;background: #fff;border: solid 2px black;overflow: hidden;font-weight: bold;font-size: larger;} .cell{width: 33%;height: 35px;display: inline-block;background: #fff;border: solid 2px black;overflow: hidden;} </style> <body> <p>Dear Sir/Madam, </p><p>Greetings from REpro Services!!</p> <p>We wish to inform you that, the Income tax department has mandated all banks to migrate to their new portal and this has a bearing on the TDS payments which we were doing U/s. 194IA on your behalf. </p><br> " +

        //  " <p>The key change impacting us is that now the Form 26QB can be filled only after logging into the Income tax portal account of every buyer. To continue managing your TDS compliance by Repro services, we need your Income tax Login password of all owners. </p><br>" +

        //" <p>Hence, request you to fill the information below and respond to this email at the earliest to ensure seamless compliance within the stipulated timelines. </p><br>" + table +
       
        //" <p>If your PAN is not yet registered in the Income tax portal or if you do not remember your Income tax Login password, we request you to use the below relevant link to know the process. Using the same, request you to either create or reset the password and share it with us for due compliance. </p><br>" +
        
        //" <p>Link for how to register PAN in Income tax portal – <a href='https://www.incometax.gov.in/iec/foportal/help/how-to-register-e-filing'> https://www.incometax.gov.in/iec/foportal/help/how-to-register-e-filing </a> </p><br>" +
        
        //" <p>Link for how to Reset Income tax Login password –<a href='https://www.incometax.gov.in/iec/foportal/help/how-to-reset-e-filing-password'> https://www.incometax.gov.in/iec/foportal/help/how-to-reset-e-filing-password</a> </p><br>" +
        
        //" <p>If the TDS compliance for your unit is already completed in all respects, please ignore this email. </p><br>" +
        
        //" <p>Feel free to get in touch with us for any further information/clarification if required.</p><br>" +
        
        //"<br> <img height='90' width='170'  src=cid:added-image-id><p>Thanks and Regards,<br>REpro Team</p> </body></html> ";


        //    EmailHelper emailHelper = new EmailHelper(_configuration);
        //    var isSent = emailHelper.SendEmail(emilaModel, logoResource);
        //    return isSent;
        //}
    }
}