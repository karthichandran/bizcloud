using System;
using System.Collections.Generic;

namespace ReProServices.Domain.Entities
{
    public class Property
    {
		public Property()
		{
			//CustomerProperty = new HashSet<CustomerProperty>();
			//PropertyModLog = new HashSet<PropertyModLog>();
        }

		public int PropertyID { get; set; }
		public int PropertyType { get; set; }
		public string AddressPremises { get; set; }
		public string AddressLine1 { get; set; }
		public string AddressLine2 { get; set; }
		public string City { get; set; }
		public string PinCode { get; set; }
		public decimal TdsInterestRate { get; set; }
		public decimal? LateFeePerDay { get; set; }
		public int StateID { get; set; }
		public int GstTaxCode { get; set; }
		public int TDSTaxCode { get; set; }

		public string PropertyCode { get; set; }

		public string PropertyShortName { get; set; }
		public bool? IsActive { get; set; }
		public DateTime? Created { get; set; }
		public string CreatedBy { get; set; }
		public DateTime? Updated { get; set; }
		public string UpdatedBy { get; set; }

		//[InverseProperty("Property")]
		//public virtual ICollection<CustomerProperty> CustomerProperty { get; set; }
		//[InverseProperty("PropertyIdFkNavigation")]
		//public virtual ICollection<PropertyModLog> PropertyModLog { get; set; }
		//[InverseProperty("PropertyIdFkNavigation")]
		public virtual ICollection<SellerProperty> SellerProperty { get; set; } = new HashSet<SellerProperty>();
    }
}
