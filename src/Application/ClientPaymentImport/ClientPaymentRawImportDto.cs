using System;
using AutoMapper;
using CsvHelper.Configuration.Attributes;
using ReProServices.Application.Common.Mappings;

namespace ReProServices.Domain.Entities
{
    public class ClientPaymentRawImportDto : IMapFrom<ClientPaymentRawImport>
	{
		[Index(0)]
		public string PropertyCode { get; set; }
		[Index(1)]
		public string ReceiptNo { get; set; }
		[Index(2)]
		public string DateOfPayment { get; set; }
		[Index(3)]
		public string RevisedDateOfPayment { get; set; }
		[Index(4)]
		public string UnitNo { get; set; }
		[Index(5)]
		public int? LotNo { get; set; }
		[Index(6)]
		public string AmountPaid { get; set; }
		[Index(7)]
		public string Name1 { get; set; }
		[Index(8)]
		public string NatureOfPayment { get; set; }
		[Index(9)]
		public string NotToBeConsideredReason { get; set; }

		public void Mapping(Profile profile)
		{
			profile.CreateMap<ClientPaymentRawImport, ClientPaymentRawImportDto>();
		}
	}
}
