using AutoMapper;
using ReProServices.Application.Common.Mappings;
using System;
using System.ComponentModel.DataAnnotations;

namespace ReProServices.Application.CustomerBillings
{
    public class CustomerBillingDto : IMapFrom<Domain.Entities.CustomerBilling>
    {
        [Key]
        public int CustomerBillID { get; set; }
        public Guid OwnershipID { get; set; }
        public bool CoOwner { get; set; }
        public int PayableBy { get; set; }
        public decimal? Amount { get; set; }
        public decimal GstRate { get; set; }
        public decimal? GstAmount { get; set; }
        public decimal? TotalPayable { get; set; }
        public int CustomerID { get; set; }
        public int PaymentMethodID { get; set; }
        public int? NoOfInstallments { get; set; }
        public decimal? CostPerInstallment { get; set; }
        public DateTime BillDate { get; set; }
      
        public virtual string CustomerName { get; set; }
        public virtual int PropertyID { get; set; }
        public virtual string PropertyPremises { get; set; }
        public virtual int UnitNo { get; set; }
        public virtual string PayableByText { get; set; }
        public virtual string  PaymentMethodText { get; set; }
        public virtual string PAN { get; set; }
        public void Mapping(Profile profile)
        {
            profile.CreateMap<Domain.Entities.CustomerBilling, CustomerBillingDto>();
        }
    }
}
