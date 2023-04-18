using System;
using System.Collections.Generic;

namespace ReProServices.Domain.Entities
{
    public class Seller 
	{
		public int SellerID { get; set; }
		public string SellerName { get; set; }
		public string AddressPremises { get; set; }
		public string AdressLine1 { get; set; }
		public string AddressLine2 { get; set; }
		public string City { get; set; }
		public string PinCode { get; set; }
		public string EmailID { get; set; }
		public string PAN { get; set; }
		public string MobileNo { get; set; }
		public int StateID { get; set; }
		public bool IsResident { get; set; }
		public DateTime? Created { get; set; }
		public string CreatedBy { get; set; }
		public DateTime? Updated { get; set; }
		public string UpdatedBy { get; set; }
		//[ForeignKey(nameof(StateId))]
		//[InverseProperty(nameof(StateList.Seller))]
		//public virtual StateList State { get; set; }
		//[InverseProperty("SellerIdFkNavigation")]
		public virtual ICollection<SellerModLog> SellerModLog { get; set; } = new HashSet<SellerModLog>();

        //[InverseProperty("SellerIdFkNavigation")]
		public virtual ICollection<SellerProperty> SellerProperty { get; set; } = new HashSet<SellerProperty>();
    }
}
