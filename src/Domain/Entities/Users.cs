using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ReProServices.Domain.Entities
{
   public class Users
    {
        [Key]
        public int UserID { get; set; }
        public string UserName { get; set; }
        public string LoginName { get; set; }
        public string UserPassword { get; set; }
        public string Code { get; set; }
        public string Email { get; set; }
        public string MobileNo { get; set; }
        public int? GenderID { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public bool IsActive { get; set; }
        public bool? IsAgent { get; set; }
        public string ISD { get; set; }
        public bool IsAdmin { get; set; }
       // public DateTime? Created { get; set; }
       // public string CreatedBy { get; set; }
       // public DateTime? Updated { get; set; }
       // public string UpdatedBy { get; set; }

    }
}
