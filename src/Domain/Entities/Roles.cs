using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ReProServices.Domain.Entities
{
   public class Roles
    {
        [Key]
        public int RoleID { get; set; }
        public string Name { get; set; }
        public string ReportingTo { get; set; }
        public bool IsOrganizationRole { get; set; }
        public DateTime? Created { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? Updated { get; set; }
        public string UpdatedBy { get; set; }
    }
}
