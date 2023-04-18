using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ReProServices.Application.LotSummaryReport;
using ReProServices.Application.LotSummaryReport.Query;
using WeihanLi.Npoi;

namespace WebApi.Controllers
{
    [Authorize(Roles = "LotSummaryReport_View")]
    public class LotSummaryReportController : ApiController
    {
        [HttpGet()]
        public async Task<IList<LotSummaryDto>> Get()
        {
            return await Mediator.Send(new GetLotSummaryReportQuery() { });
        }

        [HttpGet("getExcel")]
        public async Task<FileResult> GetReport()
        {
            var result = await Mediator.Send(new GetLotSummaryReportQuery() { });
            var settings = FluentSettings.For<LotSummaryDto>();
            settings.HasAuthor("REpro Services");
            settings.HasFreezePane(0, 1);
            settings.HasSheetConfiguration(0, "sheet 1", 1, true);
           
            settings.Property(x => x.LotNo)
                    .HasColumnTitle("Lot No.")
                    .HasColumnIndex(0);

            settings.Property(x => x.TotalPayments)
                    .HasColumnTitle("Total No. of Units")
                    .HasColumnIndex(1);

            settings.Property(x => x.PaymentsConsidered)
                  .HasColumnTitle("No. of Units Considered")
                  .HasColumnIndex(2);

            settings.Property(x => x.PaymentsNotConsidered)
                    .HasColumnTitle("No. of Units Not Considered")
                    .HasColumnIndex(3);

            //settings.Property(x => x.TransactionsCount)
            //       .HasColumnTitle("Total Payments")
            //       .HasColumnIndex(4);

            settings.Property(x => x.TransactionConsidered)
                  .HasColumnTitle("No. of Payments Considered")
                  .HasColumnIndex(5);

            settings.Property(x => x.TransactionNotConsidered)
                  .HasColumnTitle("No. of Payments Not Considered")
                  .HasColumnIndex(6);

            settings.Property(x => x.TransWithTdsPending)
                    .HasColumnTitle("No. of TDS Payments to be Done")
                    .HasColumnIndex(7);

            settings.Property(x => x.TransWithTdsPaid)
                    .HasColumnTitle("No. of TDS Payments Completed")
                    .HasColumnIndex(8);

            settings.Property(x => x.TransWithCoOwner)
                    .HasColumnTitle("No. of Payments with Co-Owners")
                    .HasColumnIndex(9);

            settings.Property(x => x.TransWithNoCoOwner)
                    .HasColumnTitle("No. of Payments without Co-Owners")
                    .HasColumnIndex(10);

            settings.Property(x => x.TransWithF16BGenerated)
                    .HasColumnTitle("No. of Records Form 16B Generated")
                    .HasColumnIndex(11);

            settings.Property(x => x.TransWithF16BGeneratedWithSharedOwnership)
                    .HasColumnTitle("No. of Records Form 16B Generated (With Co-Owners)")
                    .HasColumnWidth(50)
                    .HasColumnIndex(12); ;

            settings.Property(x => x.TransWithF16BGeneratedWithNotSharedOwnership)
                    .HasColumnTitle("No. of Records Form 16B Generated (Without Co-Owners)")
                    .HasColumnWidth(50)
                    .HasColumnIndex(13);

            var ms = result.ToExcelBytes();

            return File(ms, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "LotSummaryReport.xls");

        }

    }
}