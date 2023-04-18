using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ReProServices.Domain.Entities
{
    public class SegmentRolePermissions
    {[Key]
        public int SegmentRolePermissionsID { get; set; }
        public int RoleID { get; set; }
        public int SegmentID { get; set; }
        public bool CreatePerm { get; set; }
        public bool EditPerm { get; set; }
        public bool DeletePerm { get; set; }
        public bool ViewPerm { get; set; }
        //public DateTime? Created { get; set; }
        //public string CreatedBy { get; set; }
        //public DateTime? Updated { get; set; }
        //public string UpdatedBy { get; set; }
    }
}
