using Microsoft.AspNetCore.Mvc;
using ReProServices.Application.CustomerPropertyFiles;
using ReProServices.Application.DebitAdvices;
using ReProServices.Application.DebitAdvices.Commands;
using ReProServices.Application.DebitAdvices.Queries;
using ReProServices.Domain;
using ReProServices.Infrastructure.GoogleDrive;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace WebApi.Controllers
{
    public class DebitAdviceController : ApiController
    {
        private DriverService driverSrv;
        public DebitAdviceController()
        {
            driverSrv = new DriverService();
        }

        [HttpPost]
        public async Task<IActionResult> Create( CreateDebitAdviceCommand command )
        {
            try {
                var result = await Mediator.Send(command);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex}");
            }
          
        }

        [HttpPut]
        public async Task<IActionResult> Update(UpdateDebitAdviceCommand command)
        {
            try
            {
                var result = await Mediator.Send(command);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }

        }

        [HttpPost("uploadFile"), DisableRequestSizeLimit]
        public async Task<IActionResult> UploadRemittanceFile()
        {
            try
            {
                var file = Request.Form.Files.FirstOrDefault();
                if (file.Length == 0)
                {
                    throw new DomainException("One of the files is empty or  corrupt");
                }

                CustomerPropertyFileDto custPropFile = new CustomerPropertyFileDto
                {
                    FileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"'),
                };

                var ms = new MemoryStream();
                await file.OpenReadStream().CopyToAsync(ms);               
                custPropFile.FileBlob = new byte[1];
                custPropFile.FileType = file.ContentType;
                custPropFile.FileCategoryId = 8; //it denotes debit advice

                var gdid = await driverSrv.AddFile(custPropFile.FileName, custPropFile.FileType, ms);
                custPropFile.GDfileID = gdid;

                if (string.IsNullOrEmpty(gdid))
                    throw new DomainException("File Upload is failed.");


                var result = await Mediator.Send(new DebitAdviceUploadFileCommand { CustomerPropertyFile= custPropFile });
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex}");
            }
        }

        [HttpGet("getById/{clientpaymentTransId}")]
        public async Task<IActionResult> GetDAByClientPaymentTransId(int clientpaymentTransId)
        {
            try
            {
                var result = await Mediator.Send(new GetDebitAdviceByClientPaymentTransIdQuery { clientPaymentTransactionId= clientpaymentTransId });
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex}");
            }

        }


        [HttpDelete("{clientpaymentTransId}")]
        public async Task<IActionResult> DeleteDebitAdvice(int clientpaymentTransId)
        {
            try
            {
                var result = await Mediator.Send(new DeleteDebitAdviceCommand { ClientPaymentTransactionId = clientpaymentTransId });
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex}");
            }

        }
    }
}
