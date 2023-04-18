using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ReProServices.Domain.Entities
{
    public class Customer
    {
        [Key]
        [Column("CustomerID")]
        public int CustomerID { get; set; }
        [Required]
        public string Name { get; set; }
        public string PAN { get; set; }
        public string AddressPremises { get; set; }
        public string AdressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string City { get; set; }
        public string PinCode { get; set; }
        public string MobileNo { get; set; }
        public string EmailID { get; set; }
        [Column(TypeName = "date")]
        public DateTime DateOfBirth { get; set; }
        public int? StateId { get; set; }
        public bool IsTracesRegistered { get; set; }
        public string TracesPassword { get; set; }
        public bool? AllowForm16B { get; set; }
        public bool? IsPanVerified { get; set; }
        public string ISD { get; set; }
        public string AlternateNumber { get; set; }
        public bool? OnlyTDS { get; set; }
        public string IncomeTaxPassword { get; set; }
        //public DateTime? Created { get; set; }
        //public string CreatedBy { get; set; }
        //public DateTime? Updated { get; set; }
        //public string UpdatedBy { get; set; }
        public virtual ICollection<CustomerModLog> CustomerModLog { get; set; } = new HashSet<CustomerModLog>();
        public virtual ICollection<CustomerProperty> CustomerProperty { get; set; } = new HashSet<CustomerProperty>();
    }
}
