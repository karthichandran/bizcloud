using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ReProServices.Application.PasswordSettingReport;
using ReProServices.Application.PasswordSettingReport.Queries;
using WeihanLi.Npoi;


namespace WebApi.Controllers
{
    [Authorize(Roles = "PasswordSettingReport_View")]
    public class PasswordSettingReportController : ApiController
    {
        [HttpGet()]
        public async Task<IList<TracesPasswordDto>> Get([FromQuery] TracesPasswordReportFilter statusReportFilter)
        {
            return await Mediator.Send(new TracesPasswordReportQuery() { Filter = statusReportFilter });
        }
        [HttpGet("getExcel")]
        public async Task<FileResult> GetReport([FromQuery] TracesPasswordReportFilter statusReportFilter)
        {
            var result = await Mediator.Send(new TracesPasswordReportQuery() { Filter = statusReportFilter });
            var settings = FluentSettings.For<TracesPasswordDto>();
            settings.HasAuthor("REpro Services");
            settings.HasFreezePane(0, 1);
            settings.HasSheetConfiguration(0, "sheet 1", 1, true);
            settings.Property(x => x.LotNumber)
               .HasColumnTitle("Lot Number")
               .HasColumnIndex(0);

            settings.Property(x => x.UnitNo)
            .HasColumnTitle("UNit No")
            .HasColumnWidth(36)
            .HasColumnIndex(1);

            settings.Property(x => x.PropertyName)
                .HasColumnTitle("Premises")
                .HasColumnWidth(36)
                .HasColumnIndex(2);

            settings.Property(x => x.HasTracesPassword)
               .HasColumnTitle("Traces Password Y/N")
               .HasColumnIndex(3);

            settings.Property(x => x.Pan)
                .HasColumnTitle("PAN Number")
                .HasColumnWidth(14)
                .HasColumnIndex(4);

            settings.Property(x => x.DateOfBirth)
                .HasColumnTitle("Date Of Birth")
                .HasColumnFormatter("dd-MMM-yyy")
                .HasColumnWidth(21)
                .HasColumnIndex(5);

            settings.Property(x => x.NameInChallan)
               .HasColumnTitle("Name as per the TDS paid Challan")
               .HasColumnIndex(6);

            settings.Property(x => x.NameInSystem)
              .HasColumnTitle("Name as per system")
              .HasColumnWidth(21)
              .HasColumnIndex(7);

            settings.Property(x => x.ChallanSerialNo)
             .HasColumnTitle("Challan Serial No")
             .HasColumnWidth(150)
             .HasColumnIndex(8);
            settings.Property(x => x.AddressPremises)
             .HasColumnTitle("Address Premises")
             .HasColumnIndex(9);
            settings.Property(x => x.Address1)
            .HasColumnTitle("Address 1")
            .HasColumnIndex(10);

            settings.Property(x => x.Address2)
            .HasColumnTitle("Address 2")
            .HasColumnIndex(11);
            settings.Property(x => x.City)
            .HasColumnTitle("City")
            .HasColumnIndex(12);
            settings.Property(x => x.Pincode)
                        .HasColumnTitle("PinCode")
                        .HasColumnIndex(13);

            settings.Property(_ => _.CustomerId).Ignored();
            settings.Property(_ => _.PropertyId).Ignored();
          
            var ms = result.ToExcelBytes();

            return File(ms, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "PasswordSettingReport.xls");

        }
    }
}
