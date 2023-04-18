using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ReProServices.Domain.Entities
{
    public class UserRoles
    {
        [Key]
        public int UserRoleID { get; set; }
        public int UserID { get; set; }
        public int RoleID { get; set; }
        //public DateTime? Created { get; set; }
        //public string CreatedBy { get; set; }
        //public DateTime? Updated { get; set; }
        //public string UpdatedBy { get; set; }
    }
}
