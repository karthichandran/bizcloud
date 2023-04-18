using System;
using System.Collections.Generic;
using System.Text;

namespace ReProServices.Application.Prospect
{
    public class ProspectListVM
    {
        public int ProspectPropertyID { get; set; }
        public string name { get; set; }
        public DateTime DeclarationDate { get; set; }
        public int propertyID { get; set; }
        public string Premises { get; set; }
        public int unitNo { get; set; }
    }
}
