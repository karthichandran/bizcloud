using AutoMapper;
using ReProServices.Application.Common.Mappings;
using System;

namespace ReProServices.Application.CustomerProperty

{
    public class CustomerPropertyDto : IMapFrom<Domain.Entities.CustomerProperty>
    {
        public int CustomerPropertyId { get; set; }
        public decimal? CustomerShare { get; set; }
        public int CustomerId { get; set; }
        public int PropertyId { get; set; }
        public DateTime DateOfSubmission { get; set; }
        public int? UnitNo { get; set; }
        public string Remarks { get; set; }
        public bool IsShared { get; set; }
        public int? StatusTypeId { get; set; }
        public int? PaymentMethodId { get; set; }
        public int GstRateID { get; set; }
        public int TdsRateID { get; set; }
        public decimal? TotalUnitCost { get; set; }
        public bool? TdsCollectedBySeller { get; set; }
        public Guid? OwnershipID { get; set; }
        public string CustomerAlias { get; set; }
        public DateTime? DateOfAgreement { get; set; }
        public bool IsPrimaryOwner {get;set;}

        public bool? IsArchived { get; set; }
        public decimal? StampDuty { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Domain.Entities.CustomerProperty, CustomerPropertyDto>();
        }


    }
}
