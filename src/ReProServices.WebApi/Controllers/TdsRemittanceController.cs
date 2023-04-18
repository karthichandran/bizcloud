using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ReProServices.Application.ClientPayments.Commands;
using ReProServices.Application.ClientPayments.Commands.DeleteClientPaymentCommand;
using ReProServices.Application.Remittances;
using ReProServices.Application.Remittances.Commands.CreateRemittance;
using ReProServices.Application.Remittances.Commands.UpdateRemittance;
using ReProServices.Application.Remittances.Queries;
using ReProServices.Application.TdsRemittance;
using ReProServices.Application.TdsRemittance.Queries;
using ReProServices.Application.TdsRemittance.Queries.GetRemittanceList;
using ReProServices.Domain.Enums;
using WeihanLi.Npoi;

namespace WebApi.Controllers
{
    //[Authorize]
    public class TdsRemittanceController : ApiController
    {
        //end point for tds remittance web screen searh list
        [HttpGet]
        public async Task<IList<TdsRemittanceDto>> Get([FromQuery]TdsRemittanceFilter tdsRemittanceFilter)
        {
           return await Mediator.Send(new GetRemittanceListQuery() { Filter = tdsRemittanceFilter });
        }

        //end point for Form26QB desktop grid
        [HttpGet("pendingTds")]
        public async Task<IList<TdsRemittanceDto>> GetTdsPending([FromQuery]TdsRemittanceFilter tdsRemittanceFilter)
        {
            return await Mediator.Send(new GetTdsPendingRemittanceListQuery() { Filter = tdsRemittanceFilter });
        }

        //End point used for traces grid in desktop app
        [HttpGet("processedList")]
        public async Task<IList<TdsRemittanceDto>> GetProcessed([FromQuery]TdsRemittanceFilter tdsRemittanceFilter)
        {
            return await Mediator.Send(new GetProcessedRemittanceListQuery() { Filter = tdsRemittanceFilter });
        }

        //End point to fill and download traces doc
        [HttpGet("getRemittance/{clientPayTransId}")]
        public async Task<RemittanceDto> GetRemittanceByTransactionID(int clientPayTransId)
        {
            return await Mediator.Send(new GetRemittancesQuery() { ClientPaymentTransactionID = clientPayTransId });
        }

        //End point to get seller pan
        [HttpGet("getSellerPan/{clientPayTransId}")]
        public async Task<string> GetSellerPanByTransactionID(int clientPayTransId)
        {
            return await Mediator.Send(new GetSellerPanByTransactionIdQuery() { TransactionID = clientPayTransId });
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateRemittaneCommand command)
        {
            var result = await Mediator.Send(command);
            await Mediator.Send(new UpdateRemittanceStatusCommand
            { ClientPaymentTransactionID = command.RemittanceDto.ClientPaymentTransactionID, StatusID = command.RemittanceDto.RemittanceStatusID });
            return Ok(result) ;
        }

        [HttpPut]
        public async Task<IActionResult> Update(UpdateRemittaneCommand command)
        {
            var result = await Mediator.Send(command);

            if (!string.IsNullOrEmpty(command.RemittanceDto.ChallanID))
            {
                if (!string.IsNullOrEmpty(command.RemittanceDto.F16BCertificateNo))
                {
                  var  result1 =  await Mediator.Send(new UpdateRemittanceStatusCommand 
                        { ClientPaymentTransactionID = command.RemittanceDto.ClientPaymentTransactionID, StatusID = (int)ERemittanceStatus.Form16BDownloaded });
                  return Ok(result1);
                }

                if (!string.IsNullOrEmpty(command.RemittanceDto.F16BRequestNo))
                {
                   var result2 = await Mediator.Send(new UpdateRemittanceStatusCommand
                        { ClientPaymentTransactionID = command.RemittanceDto.ClientPaymentTransactionID, StatusID = (int)ERemittanceStatus.Form16BRequested });
                    return Ok(result2);
                }

                if (!string.IsNullOrEmpty(command.RemittanceDto.ChallanAckNo))
                {
                    var result3 = await Mediator.Send(new UpdateRemittanceStatusCommand 
                        { ClientPaymentTransactionID = command.RemittanceDto.ClientPaymentTransactionID, StatusID = (int)ERemittanceStatus.TdsPaid });
                    return Ok(result3);
                }
            }

            return Ok(result);
        }

        [Authorize(Roles = "TdsRemittance_View")]
        [HttpGet("report")]
        public async Task<IList<RemittanceReportDto>> GetRemittanceReport([FromQuery]RemittanceReportFilter remittanceFilter)
        {
            return await Mediator.Send(new GetRemittanceReportQuery() { Filter = remittanceFilter });
        }

        [Authorize(Roles = "TdsRemittance_View")]
        [HttpGet("getExcel")]
        public async Task<FileResult> GetReport([FromQuery] RemittanceReportFilter remittanceFilter)
        {
            var result = await Mediator.Send(new GetRemittanceReportQuery() { Filter = remittanceFilter });
            var settings = FluentSettings.For<RemittanceReportDto>();
            settings.HasAuthor("REpro Services");
            settings.HasFreezePane(0, 1);
            settings.HasSheetConfiguration(0, "sheet 1", 1, true);
            settings.Property(x => x.LotNo)
               .HasColumnTitle("Lot No")
               .HasColumnIndex(0);

            settings.Property(x => x.CustomerName)
              .HasColumnTitle("Client Name")
              .HasColumnIndex(1);

            settings.Property(x => x.Premises)
            .HasColumnTitle("Project Name")
            .HasColumnWidth(36)
            .HasColumnIndex(2);

            settings.Property(x => x.UnitNo)
                .HasColumnTitle("Unit No")
                .HasColumnWidth(14)
                .HasColumnIndex(3);

            settings.Property(x => x.DateOfPayment)
                .HasColumnTitle("Date Of Payment")
                .HasColumnFormatter("dd-MMM-yyy")
                .HasColumnWidth(21)
                .HasColumnIndex(4);

            settings.Property(x => x.AmountPaid)
                .HasColumnTitle("Amount Paid")
                .HasColumnWidth(21)
                .HasColumnIndex(5);

            settings.Property(x => x.GST)
                .HasColumnTitle("GST")
                .HasColumnWidth(21)
                .HasColumnIndex(6);

            settings.Property(x => x.GrossAmount)
                .HasColumnTitle("Gross Amount")
                .HasColumnWidth(21)
                .HasColumnIndex(7);

            settings.Property(x => x.TdsRate)
                .HasColumnTitle("TDS %")
                .HasColumnWidth(21)
                .HasColumnIndex(8);

            settings.Property(x => x.F16CreditedAmount)
            .HasColumnTitle("TDS Amount")
            .HasColumnWidth(21)
            .HasColumnIndex(9);

            settings.Property(x => x.ChallanDate)
                .HasColumnTitle("Challan Date")
                .HasColumnFormatter("dd-MMM-yyy")
                .HasColumnWidth(21)
                .HasColumnIndex(10);

            settings.Property(x => x.ChallanID)
               .HasColumnTitle("Challan Sl. No")
               .HasColumnWidth(14)
               .HasColumnIndex(11);

            settings.Property(x => x.ChallanAckNo)
               .HasColumnTitle("TDS Ack No")
               .HasColumnWidth(21)
               .HasColumnIndex(12);

            settings.Property(x => x.ChallanAmount)
              .HasColumnTitle("Challan Amount")
              .HasColumnWidth(21)
              .HasColumnIndex(13);

            settings.Property(x => x.F16BDateOfReq)
             .HasColumnTitle("Date Of Request")
             .HasColumnFormatter("dd-MMM-yyy")
             .HasColumnWidth(21)
             .HasColumnIndex(14);

            settings.Property(x => x.F16BRequestNo)
              .HasColumnTitle("Request No")
              .HasColumnWidth(21)
              .HasColumnIndex(15);

            settings.Property(x => x.F16UpdateDate)
            .HasColumnTitle("TDS Certificate Date")
            .HasColumnFormatter("dd-MMM-yyy")
            .HasColumnWidth(21)
            .HasColumnIndex(16);

            settings.Property(x => x.F16BCertificateNo)
             .HasColumnTitle("TDS Certificate No")
             .HasColumnWidth(21)
             .HasColumnIndex(17);



            settings.Property(_ => _.RemittanceID).Ignored();
            settings.Property(_ => _.ClientPaymentTransactionID).Ignored();
            settings.Property(_ => _.RemittanceStatusID).Ignored();
            settings.Property(_ => _.Form16BlobID).Ignored();
            settings.Property(_ => _.ChallanBlobID).Ignored();
            settings.Property(_ => _.F16CustName).Ignored();
            settings.Property(_ => _.EmailSent).Ignored();
            settings.Property(_ => _.EmailSentDate).Ignored();
            settings.Property(_ => _.DateOfBirth).Ignored();
            settings.Property(_ => _.CustomerPAN).Ignored();
            settings.Property(_ => _.ChallanCustomerName).Ignored();
            settings.Property(_ => _.ChallanFeeAmount).Ignored();
            settings.Property(_ => _.ChallanIncomeTaxAmount).Ignored();
            settings.Property(_ => _.ChallanInterestAmount).Ignored();


            var ms = result.ToExcelBytes();

            return File(ms, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "TdsRemittanceReport.xls");

        }

    }
}