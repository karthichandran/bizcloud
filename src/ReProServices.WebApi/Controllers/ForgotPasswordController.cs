using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Mail;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using ReProServices.Domain;
using ReProServices.Infrastructure.Smtp;
using ReProServices.Application.ForGotPassword;
namespace WebApi.Controllers
{
    public class ForgotPasswordController : ApiController
    {
        private IConfiguration _configuration;

        public ForgotPasswordController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpPost]
        public async Task<ActionResult> Update(ForgotPasswordDto dto)
        {
            var result = await Mediator.Send(new CheckUserIsExistQuery { Filter=dto});

            if (result==null)
                throw new ApplicationException("User Name and Email are not valid ");


            var filePath = @Directory.GetCurrentDirectory() + "\\Resources\\logo.png";
            Bitmap b = new Bitmap(filePath);
            MemoryStream ms = new MemoryStream();
            b.Save(ms, ImageFormat.Png);
            ms.Position = 0;
            var logoResource = new LinkedResource(ms, "image/png") { ContentId = "added-image-id" };

           

           
            var subject = "Recover Password";
            var emilaModel = new EmailModel()
            {
                To = dto.Email,
              // To = "karthi@leansys.in",
                Subject = subject,
                Message = @"<html><body> <p>Dear Sir/Madam, </p> <p>Greetings from REpro Services!!</p>  <p>Please use the following credential to login.</p>" +
                "<p> Login Name : "+result.LoginName+"</p>"+
                "<p> PassWord :"+result.UserPassword+ "</p><br/><br/>" +
                "<img height='90' width='170'  src=cid:added-image-id><p>Thanks and Regards,<br>R Ganesh / Sriram B Iyer <br>+91 9620508968 / +91 9663751471</p>  </body></html> ",
                IsBodyHtml = true
            };

            EmailHelper emailHelper = new EmailHelper(_configuration);
            emailHelper.SendEmail(emilaModel, logoResource);
            return NoContent();

        }

        [HttpPost("testmail")]
        public ActionResult TestMail()
        {
            var filePath = @Directory.GetCurrentDirectory() + "\\Resources\\logo.png";
            Bitmap b = new Bitmap(filePath);
            MemoryStream ms = new MemoryStream();
            b.Save(ms, ImageFormat.Png);
            ms.Position = 0;
            var logoResource = new LinkedResource(ms, "image/png") { ContentId = "added-image-id" };
            var subject = "Recover Password";
            var emilaModel = new EmailModel()
            {
               
                To = "karthi@leansys.in",
                Subject = subject,
                Message = @"<html><body> <p>Dear Sir/Madam, </p> <p>Greetings from REpro Services!!</p>  <p>Please use the following credential to login.</p>" +
                "<p> Login Name : karthi </p>" +
                "<p> PassWord : karthi</p><br/><br/>" +
                "<img height='90' width='170'  src=cid:added-image-id><p>Thanks and Regards,<br>R Ganesh / Sriram B Iyer <br>+91 9620508968 / +91 9663751471</p>  </body></html> ",
                IsBodyHtml = true
            };

            EmailHelper emailHelper = new EmailHelper(_configuration);
            emailHelper.SendEmail(emilaModel, logoResource);
            return NoContent();

        }
    }
}
