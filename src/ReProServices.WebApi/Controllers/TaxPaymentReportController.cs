using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ReProServices.Application.PasswordSettingReport;
using ReProServices.Application.PasswordSettingReport.Queries;
using ReProServices.Application.TaxPaymentReport;
using ReProServices.Application.TaxPaymentReport.Queries;
using WeihanLi.Npoi;


namespace WebApi.Controllers
{
    [Authorize(Roles = "PasswordSettingReport_View")]
    public class TaxPaymentReportController : ApiController
    {
        [HttpGet()]
        public async Task<IList<TaxPaymentDto>> Get([FromQuery] TaxPaymentReportFilter statusReportFilter)
        {
            return await Mediator.Send(new TaxPaymentReportQuery() { Filter = statusReportFilter });
        }
        [HttpGet("getExcel")]
        public async Task<FileResult> GetReport([FromQuery] TaxPaymentReportFilter statusReportFilter)
        {
            var result = await Mediator.Send(new TaxPaymentReportQuery() { Filter = statusReportFilter });
            var settings = FluentSettings.For<TaxPaymentDto>();
            settings.HasAuthor("REpro Services");
            settings.HasFreezePane(0, 1);
            settings.HasSheetConfiguration(0, "sheet 1", 1, true);
            settings.Property(x => x.LotNumber)
               .HasColumnTitle("Lot Number")
               .HasColumnIndex(0);

            settings.Property(x => x.PropertyName)
            .HasColumnTitle("Project Name")
            .HasColumnIndex(1);

            settings.Property(x => x.UnitNo)
               .HasColumnTitle("Unit No")
               .HasColumnIndex(2);

            settings.Property(x => x.CustomerName)
                .HasColumnTitle("Customer Name")
                .HasColumnIndex(3);

            settings.Property(x => x.NameInChallan)
              .HasColumnTitle("Name as per the TDS paid Challan")
              .HasColumnIndex(4);

            settings.Property(x => x.ChallanPaymentDate)
                .HasColumnTitle("Challan Payment Date")
                .HasColumnFormatter("dd-MMM-yyy")
                .HasColumnWidth(21)
                .HasColumnIndex(5);

            settings.Property(x => x.ChallanSerialNo)
            .HasColumnTitle("Challan Serial No")
            .HasColumnIndex(6);

            settings.Property(x => x.ChallanIncomeTaxAmount)
              .HasColumnTitle("Challan Income Tax Amount")
              .HasColumnIndex(7);

           
            settings.Property(x => x.ChallanInterestAmount)
             .HasColumnTitle("Challan Interest Amount")
             .HasColumnIndex(8);

            settings.Property(x => x.ChallanFeeAmount)
             .HasColumnTitle("Challan Fee Amount")
             .HasColumnIndex(9);

            settings.Property(x => x.ChallanTotalAmount)
             .HasColumnTitle("Challan Total Amount")
             .HasColumnIndex(10);

            settings.Property(_ => _.CustomerId).Ignored();
            settings.Property(_ => _.PropertyId).Ignored();

            var ms = result.ToExcelBytes();

            return File(ms, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "TaxPaymentReport.xls");

        }
    }
}
