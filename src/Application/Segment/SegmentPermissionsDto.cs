using System;
using System.Collections.Generic;
using System.Text;

namespace ReProServices.Application.Segment
{
    public class SegmentPermissionsDto
    {
        public int SegmentRolePermissionsID { get; set; }
        public int RoleID { get; set; }
        public int SegmentID { get; set; }
        public string Name { get; set; }        
        public bool CreatePerm { get; set; }
        public bool EditPerm { get; set; }
        public bool DeletePerm { get; set; }
        public bool ViewPerm { get; set; }
    }
}
