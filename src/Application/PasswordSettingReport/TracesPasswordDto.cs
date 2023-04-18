using System;
using System.Collections.Generic;
using System.Text;

namespace ReProServices.Application.PasswordSettingReport
{
   public class TracesPasswordDto
    {
        public int LotNumber { get; set; }
        public int UnitNo { get; set; }
        public string HasTracesPassword { get; set; }
        public int CustomerId { get; set; }
        public string Pan { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string NameInChallan { get; set; }
        public string NameInSystem { get; set; }
        public string ChallanSerialNo { get; set; }
        public string ChallanTotalAmount { get; set; }
        public string AddressPremises { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string City { get; set; }
        public string Pincode { get; set; }

        public int PropertyId { get; set; }
        public string PropertyName { get; set; }
    }
}
