using System;
using System.ComponentModel.DataAnnotations;

namespace ReProServices.Domain.Entities
{
    public class CustomerBilling
    {
        [Key]
        public int CustomerBillID { get; set; }
        public Guid OwnershipID { get; set; }
        public bool CoOwner { get; set; }
        public int PayableBy { get; set; }
        public decimal Amount { get; set; }
        public decimal GstRate { get; set; }
        public decimal GstAmount { get; set; }
        public decimal TotalPayable { get; set; }
        public int CustomerID { get; set; }
        public int PaymentMethodID { get; set; }
        public int? NoOfInstallments { get; set; }
        public decimal? CostPerInstallment { get; set; }
        public DateTime BillDate { get; set; }
        public DateTime CustomerBillCreateDate { get; set; }
        public DateTime? Created { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? Updated { get; set; }
        public string UpdatedBy { get; set; }

    }
}
