using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ReProServices.Application.StatusReport;
using ReProServices.Application.StatusReport.Queries;
using WeihanLi.Npoi;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApi.Controllers
{
    [Authorize(Roles ="StatusReport_View")]
    public class StatusReportController :  ApiController
    {
        [HttpGet()]
        public async Task<IList<StatusReportDto>> Get([FromQuery] StatusReportFilter statusReportFilter)
        {
            return await Mediator.Send(new GetStatusReportQuery() { Filter = statusReportFilter });
        }

        [HttpGet("getExcel")]
        public async Task<FileResult> GetReport([FromQuery] StatusReportFilter statusReportFilter)
        {
            var result= await Mediator.Send(new GetStatusReportQuery() { Filter = statusReportFilter });
            var settings = FluentSettings.For<StatusReportDto>();
            settings.HasAuthor("REpro Services");
            settings.HasFreezePane(0, 1);
            settings.HasSheetConfiguration(0, "sheet 1", 1, true);
            settings.Property(x => x.LotNo)
               .HasColumnTitle("Lot No.")
               .HasColumnIndex(0);

            settings.Property(x => x.Premises)
            .HasColumnTitle("Project Name")
            .HasColumnWidth(36)
            .HasColumnIndex(1);

            settings.Property(x => x.CustomerName)
               .HasColumnTitle("Client Name")
               .HasColumnIndex(2);

            settings.Property(x => x.UnitNo)
                .HasColumnTitle("Unit No")
                .HasColumnWidth(14)
                .HasColumnIndex(3);

            settings.Property(x => x.PaymentReceiptDate)
                .HasColumnTitle("Payment Receipt Date")
                .HasColumnFormatter("dd-MMM-yyy")
                .HasColumnWidth(21)
                .HasColumnIndex(4);

            settings.Property(x => x.RemittanceOfTdsAmount)
               .HasColumnTitle("Remittance of TDS amount")
               .HasColumnFormatter("dd-MMM-yyy")
               .HasColumnWidth(21)
               .HasColumnIndex(5);

            settings.Property(x => x.Form16BRequested)
              .HasColumnTitle("Form 16B Requested")
              .HasColumnFormatter("dd-MMM-yyy")
              .HasColumnWidth(21)
              .HasColumnIndex(6);

            settings.Property(x => x.Form16BDownloaded)
             .HasColumnTitle("Form 16B Downloaded")
             .HasColumnFormatter("dd-MMM-yyy")
             .HasColumnWidth(21)
             .HasColumnIndex(7);

            settings.Property(x => x.MailDate)
             .HasColumnTitle("Form 16B and Challan sent to customer")
             .HasColumnFormatter("dd-MMM-yyy")
             .HasColumnWidth(40)
             .HasColumnIndex(8);

            settings.Property(_ => _.PropertyID).Ignored();

            var ms = result.ToExcelBytes();

            return File(ms, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "StatusReport.xls");

        }

    }
}
