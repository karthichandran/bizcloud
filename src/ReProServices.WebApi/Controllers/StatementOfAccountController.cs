using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ReProServices.Application.StatementOfAccount.Queries;
using ReProServices.Domain.Entities;
using WeihanLi.Npoi;

namespace WebApi.Controllers
{
    
    public class StatementOfAccountController :   ApiController
    {
        [HttpGet()]
        public async Task<IList<StatementOfAccount>> Get([FromQuery] StatementOfAccountFilter statementOfAcctFilter)
        {
            return await Mediator.Send(new GetStatementOfAccountReport() { StatementOfAccountFilter = statementOfAcctFilter });
        }

        [HttpGet("getExcel")]
        public async Task<FileResult> GetReport([FromQuery] StatementOfAccountFilter statementOfAcctFilter)
        {
            var result = await Mediator.Send(new GetStatementOfAccountReport() { StatementOfAccountFilter = statementOfAcctFilter });

            var orderedRecords = result.OrderBy(x =>  x.OwnershipID).ThenBy(y=>y.PayableDateOfPayment);

            var settings = FluentSettings.For<StatementOfAccount>();
            settings.HasAuthor("REpro Services");
            settings.HasFreezePane(0, 1);
            settings.HasSheetConfiguration(0, "sheet 1", 1, true);
            
            settings.Property(_ => _.UnitNo)
                .HasColumnTitle("Unit No.")
                .HasColumnIndex(0);            

            settings.Property(x => x.PayableDateOfPayment)
               .HasColumnTitle("Date of Payment")
               .HasColumnFormatter("dd-MMM-yyy")
               .HasColumnWidth(18)
               .HasColumnIndex(1);

            settings.Property(x => x.PayableAmountPaid)
                .HasColumnTitle("Amount Paid")
                .HasColumnIndex(2);

            settings.Property(x => x.PayableGrossAmount)
                .HasColumnTitle("Gross Amount")
                .HasColumnIndex(3);

            settings.Property(x => x.PayableTds)
                .HasColumnTitle("TDS")
                .HasColumnWidth(36)
                .HasColumnIndex(4);

            settings.Property(x => x.PayableInterest)
                .HasColumnTitle("Interest")
                .HasColumnWidth(36)
                .HasColumnIndex(5);

            settings.Property(x => x.PayableLateFee)
                .HasColumnTitle("Late Fee")
                .HasColumnWidth(14)
                .HasColumnIndex(6);

            settings.Property(x => x.PayableServiceFee)
               .HasColumnTitle("Service Charge")
               .HasColumnWidth(16)
               .HasColumnIndex(7);

            settings.Property(x => x.ReceivedDate)
                .HasColumnTitle("Date")
                .HasColumnFormatter("dd-MMM-yyy")
                .HasColumnWidth(16)
                .HasColumnIndex(8);

            settings.Property(x => x.ReceivedTotalAmount)
               .HasColumnTitle("Total Amount Received")
               .HasColumnWidth(14)
               .HasColumnIndex(9);

            settings.Property(x => x.ReceivedTds)
              .HasColumnTitle("TDS")
              .HasColumnWidth(14)
              .HasColumnIndex(10);

            settings.Property(x => x.ReceivedInterest)
              .HasColumnTitle("Interest")
              .HasColumnWidth(14)
              .HasColumnIndex(11);

            settings.Property(x => x.ReceivedLateFee)
                .HasColumnTitle("Late Fee")
                .HasColumnWidth(14)
                .HasColumnIndex(12);

            settings.Property(x => x.ReceivedServiceCharge)
                .HasColumnTitle("Service Charge")
                .HasColumnWidth(14)
                .HasColumnIndex(13);

            settings.Property(x => x.RemittedDate)
                .HasColumnTitle("Date")
                .HasColumnFormatter("dd-MMM-yyy")  
                .HasColumnWidth(16)
                .HasColumnIndex(14);

            settings.Property(x => x.RemittedTds)
                .HasColumnTitle("TDS")
                .HasColumnWidth(14)
                .HasColumnIndex(15);

            settings.Property(x => x.RemittedInterest)
                .HasColumnTitle("Interest")
                .HasColumnWidth(14)
                .HasColumnIndex(16);

            settings.Property(x => x.RemittedLateFee)
                .HasColumnTitle("Late Fee")
                .HasColumnWidth(14)
                .HasColumnIndex(17);

            settings.Property(_ => _.OwnershipID).Ignored();
            settings.Property(_ => _.PayableGst).Ignored();
            settings.Property(_ => _.PayableReceiptNo).Ignored();
            var ms = orderedRecords.ToExcelBytes();

            return File(ms, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "StatementOfAccountReport.xls");

        }
    }
}
