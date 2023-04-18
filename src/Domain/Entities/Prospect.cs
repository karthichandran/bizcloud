using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ReProServices.Domain.Entities
{
    public class Prospect
    {
        [Key]
        public int ProspectID { get; set; }
        public int ProspectPropertyID { get; set; }
        public string Name { get; set; }
        public string AddressPremises { get; set; }
        public string PAN { get; set; }
        public int? PanBlobID { get; set; }
        public string AdressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string City { get; set; }
        public string PinCode { get; set; }
        public string MobileNo { get; set; }
        public string EmailID { get; set; }
        public DateTime DateOfBirth { get; set; }
        public int? StateId { get; set; }
        public bool IsTracesRegistered { get; set; }
        public string TracesPassword { get; set; }
        public bool? AllowForm16B { get; set; }

        public string ISD { get; set; }
        public string AlternateNumber { get; set; }
        public decimal? Share { get; set; }

        public string IncomeTaxPassword { get; set; }
    }
}
