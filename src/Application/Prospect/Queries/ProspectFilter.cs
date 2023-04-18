using System;
using System.Collections.Generic;
using System.Text;

namespace ReProServices.Application.Prospect.Queries
{
    public class ProspectFilter
    {
        public string Customer { get; set; }
        public int propertyID { get; set; }
        public string PAN { get; set; }
        public int UnitNo{get;set;}
    }
}
