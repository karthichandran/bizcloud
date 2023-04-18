using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ReProServices.Domain.Entities
{
    [Table("ViewPendingServiceFee")]
    public class ViewPendingServiceFee
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CustomerBillID { get; set; }
        public int CustomerID { get; set; }
        public Guid OwnershipID { get; set; }
        public string ClientName { get; set; }
        public decimal? ServiceFee { get; set; }
        public decimal? GstPayable { get; set; }
        public decimal? TotalPayable { get; set; }
        public string EmailID { get; set; }
        public decimal? TdsInterest { get; set; }
        public decimal? LateFee { get; set; }
        public int PropertyID { get; set; }
        public int? UnitNo { get; set; }
    }
}
