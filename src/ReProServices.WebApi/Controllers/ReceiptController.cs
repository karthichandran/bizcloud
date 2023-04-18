using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Net.Mail;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using ReProServices.Application.Receipts;
using ReProServices.Application.Receipts.Commands.CreateReceipt;
using ReProServices.Application.Receipts.Commands.UpdateReceipt;
using ReProServices.Application.Receipts.Queries;
using ReProServices.Domain;
using ReProServices.Domain.Enums;
using ReProServices.Infrastructure.Smtp;
namespace WebApi.Controllers
{
    [Authorize]
    public class ReceiptController : ApiController
    {
        private IConfiguration _configuration;
        public ReceiptController(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        [Authorize(Roles = "Receipt_View")]
        [HttpGet]
        public async Task<IList<ReceiptDto>> GetTdsReceipts([FromQuery] ReceiptFilter receiptFilter)
        {
            if (receiptFilter.IsTds)
            {
                if (receiptFilter.GetPending)
                    return await Mediator.Send(new GetPendingTdsReceiptsQuery() { Filter = receiptFilter });
                else
                    return await Mediator.Send(new GetSavedReceiptsQuery() { Filter = receiptFilter, ReceiptType = (int)EReceiptType.Tds});
            }
            else {
                if (receiptFilter.GetPending)
                    return await Mediator.Send(new GetPendingServiceFeeQuery() { Filter = receiptFilter });
                else
                    return await Mediator.Send(new GetSavedReceiptsQuery() { Filter = receiptFilter, ReceiptType = (int)EReceiptType.ServiceFee});
            }
        }
        [Authorize(Roles = "Receipt_Create")]
        [HttpPost]
        public async Task<int> Create(CreateReceiptCommand command)
        {
           return await Mediator.Send(command);
        }
        [Authorize(Roles = "Receipt_Edit")]
        [HttpPut]
        public async Task<int> Update(UpdateReceiptCommand command) {
            return await Mediator.Send(command);
        }

        //[HttpGet("sendmail/{clientPayTransIds}")]
        //public async Task<string> SendMail(string clientPayTransIds) {

        //    var ids = clientPayTransIds.Split(',');
        //    if (ids.Length == 0)
        //        throw new DomainException("No Receipt Ids Passed");
        //    string failedReceipt = "";
        //    var filePath = @Directory.GetCurrentDirectory() + "\\Resources\\logo.png";
           
        //    Bitmap b = new Bitmap(filePath);
        //    MemoryStream ms = new MemoryStream();
        //    b.Save(ms, ImageFormat.Png);
        //    ms.Position = 0;
        //    var logoResource = new LinkedResource(ms, "image/png") { ContentId = "added-image-id" };
        //    foreach (var receiptId in ids)
        //    {
        //        var model = await Mediator.Send(new GetReceiptByIdQuery() { ReceiptId = Convert.ToInt32(receiptId) });
        //        var cusModel = await Mediator.Send(new GetCoOwnersByReceiptIdQuery() { ReceiptId = Convert.ToInt32(receiptId) });
        //        var cc = "";
        //        var to = "";
        //        if (cusModel.Count > 1)
        //        {
        //            foreach (var cus in cusModel)
        //            {
        //                if (cus.isprimaryOwner)
        //                    to = cus.Email;
        //                else
        //                    cc += cus.Email + ",";
        //            }
        //        }
        //        else {
        //            to = cusModel[0].Email;
        //        }

        //        var subject = "REpro Services payment confirmation - " + model.Premises + " - " + model.UnitNo;
        //        var serviceFee = model.TotalServiceFeeReceived;
        //        var interestDelay = model.TdsInterestReceived;
        //        var lateFee = model.LateFeeReceived;
        //        var total = model.TotalServiceFeeReceived + model.TdsInterestReceived+ model.LateFeeReceived;

        //        var emilaModel = new EmailModel()
        //        {
        //           To=to,
        //           CC=cc,
        //            Subject = subject,
        //            Message = @"<html><body> <p>Dear Sir/Madam, </p><p>Greetings from REpro Services!!</p> <p>We acknowledge the receipt of your payment as under, </p> " +
        //                 " <p>REpro Service fee (Incl. GST) - Rs. " + serviceFee + " <br>Interest on delayed payment - Rs. " + interestDelay + "<br>Late fee - Rs. " + lateFee + " <br><b> Total Amount - Rs." + total + " </b></p>" +
        //                  "<p>We will coordinate with Seller and take care of your TDS compliance henceforth. Kindly ensure you pay full value of Invoice/Demand Note to the seller without deducting TDS at your end.</p>" +
        //                   "<br> <img height='90' width='170'  src=cid:added-image-id><p>Thanks and Regards,<br>R Ganesh / Sriram B Iyer <br>+91 9620508968 / +91 9663751471</p> </body></html> ",
        //            IsBodyHtml = true
        //        };

        //        EmailHelper emailHelper = new EmailHelper(_configuration);
        //       var isSent= emailHelper.SendEmail(emilaModel, logoResource);
        //        if (isSent)
        //        {
        //            await Mediator.Send(new UpdateEmailSentCommand() { ReceiptId = Convert.ToInt32(receiptId) });
        //        }
        //        else
        //            failedReceipt = receiptId + ",";
        //    }
        //     return failedReceipt;
        //}


        [HttpGet("testmail/")]
        public async Task<string> testMail()
        {           
            var filePath = @Directory.GetCurrentDirectory() + "\\Resources\\logo.png";

            Bitmap b = new Bitmap(filePath);
            MemoryStream ms = new MemoryStream();
            b.Save(ms, ImageFormat.Png);
            ms.Position = 0;
            var logoResource = new LinkedResource(ms, "image/png") { ContentId = "added-image-id" };         
                var subject = "Test Mail";               

                var emilaModel = new EmailModel()
                {
                    To = "Karthi@leansys.in",                   
                    Subject = subject,
                    Message = @"<html><body> <p>Dear Sir/Madam, </p>  </body></html> ",
                    IsBodyHtml = true
                };

                EmailHelper emailHelper = new EmailHelper(_configuration);
                var isSent = emailHelper.SendEmail(emilaModel, logoResource);
                if (isSent)
                {
                return "success";
                }
                else
            return "failed";
        }
    }
}