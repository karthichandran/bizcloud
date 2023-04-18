using System;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using ReProServices.Application.ClientPayments;
using ReProServices.Application.ClientPayments.Commands;
using ReProServices.Application.ClientPayments.Commands.DeleteClientPaymentCommand;
using ReProServices.Application.ClientPayments.Queries;
using ReProServices.Application.ClientPayments.Queries.ClientPaymentList;
using ReProServices.Application.ClientPayments.Queries.ClientPaymentReport;
using ReProServices.Application.Common.Formulas;
using ReProServices.Application.Customers;
using ReProServices.Domain.Entities;
using WeihanLi.Npoi;
using System.Linq;
using ReProServices.Domain;
using System.IO;
using LazyCache;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using ReProServices.Application.ClientPaymentImport.Commands;
using CsvHelper;
using System.Globalization;
using System.Diagnostics;
using System.Text.RegularExpressions;
using ExcelDataReader;
using ReProServices.Application.TaxCodes.Queries.GetTaxCodes;
using NodaTime.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Win32.SafeHandles;
using Microsoft.AspNetCore.SignalR;
using ReProServices.Infrastructure.HubConfig;

namespace WebApi.Controllers
{
    [Authorize]
    public class ClientPaymentController : ApiController
    {
        private readonly IAppCache _cache;
        private IHubContext<BroadcastHub> _hub;
        public ClientPaymentController(IAppCache cache, IHubContext<BroadcastHub> hub)
        {
            _cache = cache;
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
            _hub = hub;
        }

        [Authorize(Roles = "ClientPayment_View")]
        [HttpGet("{ownershipId}")]
        public ClientPaymentVM Get(Guid ownershipId)
        {
            var result = new ClientPaymentVM
            {
                ExistingInstallments =
                    Mediator.Send(new GetClientPaymentsByOwnershipIdQuery() { OwnershipId = ownershipId }).Result,
                InstallmentBaseObject = Mediator.Send(new GetBasePaymentObjectByOwnershipIDQuery()
                {
                    OwnershipId = ownershipId
                }).Result
            };
            _hub.Clients.All.SendAsync("broadcastTest", "test broadcast");
            return result;
        }

        [HttpGet()]
        public TaxesAndFees GetTaxes([FromQuery]TaxesAndFeesIp taxesInput)
        {
            var taxesObj = new TaxesAndFees()
            {
                AmountPaid = taxesInput.AmountPaid,
                DateOfPayment = Convert.ToDateTime(taxesInput.DateOfPayment),
                DateOfDeduction = Convert.ToDateTime(taxesInput.DateOfDeduction),
                IsTdsDeductedBySeller = taxesInput.IsTdsDeductedBySeller,
                GstPercentage = taxesInput.GstPercentage,
                LateFeePerDay = 200 //todo remove hardocoded value
            };

            var taxesAndFee = TaxCalculator.CalculateTaxAndFees(taxesObj);

            return taxesAndFee;
        }
        [Authorize(Roles = "ClientPayment_View")]
        [HttpGet("paymentList")]
        public async Task<CustomerVM> Get([FromQuery] ClientPaymentFilter clientPaymentFilter)
        {
            return await Mediator.Send(new GetClientPaymentListQuery() { Filter = clientPaymentFilter });
        }

        //public async Task<FileResult> GetExcel([FromQuery] CustomerDetailsFilter customerDetailsFilter)
        //{
        //    var result = await Mediator.Send(new GetCustomersQuery() { Filter = customerDetailsFilter });

        //    var resultSet = result.customersView
        //        .Where(cp => cp.StatusTypeID <= 2).ToList();

        //    var settings = FluentSettings.For<ViewCustomerPropertyBasic>();
        //    settings.HasAuthor("REpro Services");

        //    settings.Property(_ => _.CustomerName)
        //        .HasColumnTitle("Customer Name")
        //        .HasColumnWidth(50)
        //        .HasColumnIndex(0);

        //    settings.Property(x => x.PAN)
        //        .HasColumnWidth(16)
        //        .HasColumnIndex(1);

        //    settings.Property(x => x.PropertyPremises)
        //        .HasColumnTitle("Property Premises")
        //        .HasColumnWidth(30)
        //        .HasColumnIndex(2);

        //    settings.Property(x => x.UnitNo)
        //        .HasColumnTitle("Unit No")
        //        .HasColumnWidth(18)
        //        .HasColumnIndex(3);

        //    settings.Property(x => x.TotalUnitCost)
        //        .HasColumnTitle("Unit Cost")
        //        .HasColumnWidth(18)
        //        .HasColumnIndex(4);

        //    settings.Property(x => x.DateOfAgreement)
        //        .HasColumnTitle("Date of Agreement")
        //        .HasColumnFormatter("dd-MMM-yyy")
        //        .HasColumnWidth(18)
        //        .HasColumnIndex(5);

        //    settings.Property(x => x.DateOfSubmission)
        //        .HasColumnTitle("Date of Submission")
        //        .HasColumnFormatter("dd-MMM-yyy")
        //        .HasColumnWidth(18)
        //        .HasColumnIndex(6);

        //    settings.Property(x => x.Remarks)
        //        .HasColumnTitle("Remarks")
        //        .HasColumnWidth(60)
        //        .HasColumnIndex(7);

        //    settings.Property(_ => _.OwnershipID).Ignored();
        //    settings.Property(_ => _.CustomerID).Ignored();
        //    settings.Property(_ => _.PropertyID).Ignored();
        //    settings.Property(_ => _.CustomerPropertyID).Ignored();
        //    settings.Property(_ => _.OwnershipID).Ignored();
        //    settings.Property(_ => _.PaymentMethodId).Ignored();
        //    settings.Property(_ => _.StatusTypeID).Ignored();

        //    var ms = resultSet.ToExcelBytes();

        //    return File(ms, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "ClientPaymentRecords.xls");

        //}
        [Authorize(Roles = "ClientPayment_View")]
        [HttpGet("paymentList/getExcel")]
        public async Task<FileResult> GetReport([FromQuery] ClientPaymentFilter clientPaymentFilter)
        {
            var result = await Mediator.Send(new GetClientPaymentsReportQuery { Filter = clientPaymentFilter });

            var settings = FluentSettings.For<ClientPaymentReport>();
            settings.HasAuthor("REpro Services");
            settings.HasFreezePane(0, 1);
            settings.HasSheetConfiguration(0, "sheet 1", 1, true);

            settings.Property(_ => _.SlNo)
                .HasColumnTitle("Sl. No.")
                .HasColumnIndex(0);

            settings.Property(x => x.LotNo)
                .HasColumnTitle("Lot No.")
                .HasColumnIndex(1);

            settings.Property(x => x.CustomerName)
                .HasColumnTitle("Client Name")
                .HasColumnIndex(2);

            settings.Property(x => x.SellerName)
                .HasColumnTitle("Seller Name")
                .HasColumnWidth(36)
                .HasColumnIndex(3);

            settings.Property(x => x.PropertyPremises)
                .HasColumnTitle("Project Name")
                .HasColumnWidth(36)
                .HasColumnIndex(4);

            settings.Property(x => x.UnitNo)
                .HasColumnTitle("Unit No")
                .HasColumnWidth(14)
                .HasColumnIndex(5);

            settings.Property(x => x.DateOfBooking)
                .HasColumnTitle("Date of Booking")
                .HasColumnFormatter("dd-MMM-yyy")
                .HasColumnWidth(16)
                .HasColumnIndex(6);

            settings.Property(x => x.TotalUnitCost)
                .HasColumnTitle("Total Unit Cost")
                .HasColumnWidth(16)
                .HasColumnIndex(7);

            settings.Property(x => x.DateOfPayment)
                .HasColumnTitle("Date of Payment")
                .HasColumnFormatter("dd-MMM-yyy")
                .HasColumnWidth(18)
                .HasColumnIndex(8);

            settings.Property(x => x.RevisedDateOfPayment)
                .HasColumnTitle("Revised Date of Payment")
                .HasColumnFormatter("dd-MMM-yyy")
                .HasColumnWidth(21)
                .HasColumnIndex(9);

            settings.Property(x => x.DateOfDeduction)
                .HasColumnTitle("Date of Deduction")
                .HasColumnFormatter("dd-MMM-yyy")
                .HasColumnWidth(18)
                .HasColumnIndex(10);

            settings.Property(x => x.ReceiptNo)
                .HasColumnTitle("Receipt No.")
                .HasColumnWidth(14)
                .HasColumnIndex(11);

            settings.Property(x => x.ShareAmountPaid)
                .HasColumnTitle("Amount Paid")
                .HasColumnWidth(14)
                .HasColumnIndex(12);

            settings.Property(x => x.GstRate)
                .HasColumnTitle("GST Rate")
                .HasColumnWidth(14)
                .HasColumnIndex(13);

            settings.Property(x => x.Gst)
                .HasColumnTitle("GST Value")
                .HasColumnWidth(14)
                .HasColumnIndex(14);

            settings.Property(x => x.GrossShareAmount)
                .HasColumnTitle("Gross Value")
                .HasColumnWidth(14)
                .HasColumnIndex(15);

            settings.Property(x => x.TdsRate)
                .HasColumnTitle("TDS Rate")
                .HasColumnWidth(14)
                .HasColumnIndex(16);

            settings.Property(x => x.Tds)
                .HasColumnTitle("TDS Payable")
                .HasColumnWidth(14)
                .HasColumnIndex(17);

            settings.Property(x => x.TdsInterest)
                .HasColumnTitle("Interest")
                .HasColumnWidth(14)
                .HasColumnIndex(18);

            settings.Property(x => x.LateFee)
                .HasColumnTitle("Late Fee")
                .HasColumnWidth(14)
                .HasColumnIndex(19);

            settings.Property(x => x.RemittanceStatus)
                .HasColumnTitle("Remittance Status")
                .HasColumnIndex(20);

            settings.Property(x => x.NatureOfPaymentText)
                .HasColumnTitle("Remarks")
                .HasColumnWidth(60)
                .HasColumnIndex(21);
            settings.Property(x => x.ClientPaymentTransactionID)
               .HasColumnTitle("Transaction ID")
               .HasColumnWidth(60)
               .HasColumnIndex(22);

            settings.Property(x => x.ChallanDate)
              .HasColumnTitle("Challan Date")
              .HasColumnFormatter("dd-MMM-yyy")
              .HasColumnWidth(60)
              .HasColumnIndex(23);

            settings.Property(_ => _.OwnershipID).Ignored();
            settings.Property(_ => _.SellerID).Ignored();
            settings.Property(_ => _.InstallmentID).Ignored();
            settings.Property(_ => _.PropertyID).Ignored();
            settings.Property(_ => _.NatureOfPaymentID).Ignored();
            settings.Property(_ => _.RemittanceStatusID).Ignored();

            var ms = result.ToExcelBytes();

            return File(ms, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "ClientPaymentReport.xls");

        }

        [Authorize(Roles = "ClientPayment_Create")]
        [HttpPost]
        public async Task<Unit> Create(CreateClientPaymentCommand command)
        {
            return await Mediator.Send(command);
        }

        [Authorize(Roles = "ClientPayment_Edit")]
        [HttpPut("installmentID/{installmentID}")]
        public async Task<Unit> Update(UpdateClientPaymentCommand command)
        {
            return await Mediator.Send(command);
        }

        [Authorize(Roles = "ClientPayment_Delete")]
        [HttpDelete("installmentID/{installmentID}")]
        public async Task<ActionResult> Delete(Guid installmentID)
        {
            await Mediator.Send(new DeleteClientPaymentCommand { InstallmentID = installmentID });
            return NoContent();
        }

        [HttpPut("remittanceStatus/{clientPaymentTransactionID}/{statusId}")]
        public async Task<Unit> Update(int clientPaymentTransactionID, int statusId)
        {
            return await Mediator.Send(new UpdateRemittanceStatusCommand { ClientPaymentTransactionID = clientPaymentTransactionID, StatusID = statusId });
        }

        [HttpPost("uploadFile"), DisableRequestSizeLimit]
        public async Task<IActionResult> UploadPayment()
        {
            try
            {
                await Mediator.Send(new DeleteClientPaymentImportCOmmand());

                Stopwatch s2 = new Stopwatch();
                s2.Start();
                var files = Request.Form.Files;

                if (files.Any(f => f.Length == 0))
                {
                    throw new DomainException("One of the files is empty or corrupt");
                }

                var file = Request.Form.Files[0];
                var ms = new MemoryStream();

                file.CopyTo(ms);
                using (IExcelDataReader excelReader = ExcelReaderFactory.CreateReader(ms))
                {
                    var dataTable = new DataTable();
                    var filetype = excelReader.GetType().Name;
                    if (filetype == "ExcelOpenXmlReader")
                    {
                        using (IExcelDataReader reader =
                            ExcelReaderFactory.CreateOpenXmlReader(ms, new ExcelReaderConfiguration()))
                        {
                            dataTable = reader.AsDataSet().Tables[0];
                            DataRow row = dataTable.Rows[0];
                            dataTable.Rows.Remove(row); //removing the headings
                        }
                    }
                    else if (filetype == "ExcelBinaryReader")
                    {
                        using (IExcelDataReader reader =
                            ExcelReaderFactory.CreateBinaryReader(ms, new ExcelReaderConfiguration()))
                        {
                            dataTable = reader.AsDataSet().Tables[0];
                            DataRow row = dataTable.Rows[0];
                            dataTable.Rows.Remove(row); //removing the headings
                        }
                    }

                    Console.WriteLine("Records Count = " + dataTable.Rows.Count);
                    char dl = '-';
                    //var anyInvalidUnitNo = dataTable.Select().Any(r => r.ItemArray[4].ToString().Split(dl).Count() <= 2);
                    //if (anyInvalidUnitNo) {
                    //    throw new DomainException("Some of the records having invalid Reference format");
                    //}
                   
                    decimal receiptSum = 0;
                    var pattern = InstantPattern.CreateWithInvariantCulture("M/d/yyyy");
                    var payments = (from row in dataTable.AsEnumerable()
                                    select new ClientPaymentRawImport()
                                    {
                                        PropertyCode = Convert.ToString(row[0]),
                                        //UnitNo = Convert.ToInt32(Regex
                                        //    .Match(Convert.ToString(row[4]).Split(dl)[1] + Convert.ToString(row[4]).Split(dl)[2],
                                        //        @"\d+").Value),
                                        UnitNo = ExtractUnitNo(row[4].ToString()),
                                        AmountPaid = Convert.ToDecimal(row[6]) * -1,
                                        LotNo = Convert.ToInt32(row[5]),
                                        NatureOfPayment = Convert.ToString(row[8]),
                                        NotToBeConsideredReason = Convert.ToString(row[9]),
                                        Name = Convert.ToString(row[7]),
                                        ReceiptNo = Convert.ToString(row[1]),
                                        DateOfPayment = DateTime.Parse(row[2].ToString()),
                                        RevisedDateOfPayment = DateTime.Parse(row[3].ToString()),

                                    }).ToList();

                    await Mediator.Send(new ClientPaymentImportCommand { cpr = payments });
                }

                var TransformedPayments = await Mediator.Send(new GetClientPaymentsListFromImportsQuery());

                var taxCodes = await Mediator.Send(new GetTaxCodesQuery());
                //make the inserts
                await Mediator.Send(new CreateBulkClientPaymentCommand
                { ClientPaymentVMs = TransformedPayments, TaxCodes = taxCodes });
                s2.Stop();
                var s2Time = s2.Elapsed.TotalSeconds;

                //Generate Import Report 

                return Ok(true);

            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("lotNumbers")]
        public async Task<IList<LotNumbersDto>> Get()
        {
            return await Mediator.Send(new GetLotNumbersQuery() { });
        }

        private int ExtractUnitNo(string referenceCode) {
            var arr = referenceCode.Split('-');
            int unitNo;
            if (arr.Count() <= 2)
                unitNo = Convert.ToInt32(Regex.Match(arr[1], @"\d+").Value);
            else
                unitNo = Convert.ToInt32(Regex.Match(arr[1]+arr[2], @"\d+").Value);
            return unitNo;
        }
    }
}
