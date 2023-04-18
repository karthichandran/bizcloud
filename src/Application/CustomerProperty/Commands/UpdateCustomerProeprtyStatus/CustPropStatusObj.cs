using System;

namespace ReProServices.Application.CustomerProperty.Commands.UpdateCustomerProeprtyStatus
{
    public class CustPropStatusObj
    {
        public Guid OwnershipID { get; set; }
        public int StatusTypeID { get; set; }
        public string Remarks { get; set; }
    }
}
