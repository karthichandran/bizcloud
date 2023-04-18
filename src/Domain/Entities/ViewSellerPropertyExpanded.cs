using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ReProServices.Domain.Entities
{
    [Table("ViewSellerPropertyExpanded")]
    public class ViewSellerPropertyExpanded
    {
       [Key]
       [DatabaseGenerated(DatabaseGeneratedOption.Identity)] 
        public int SellerPropertyID { get; set; }
        public decimal SellerShare { get; set; }
        public int PropertyID { get; set; }
        public string PropertyPremises { get; set; }
        public int SellerID { get; set; }
        public string SellerName { get; set; }
     
        public string SellerState { get; set; }
        public string Seller26BState { get; set; }
        public string SellerPAN { get; set; }

        public string Property26BState {get;set;}
        public string PropertyState {get;set;}
        public string SellerMobileNo {get;set;}
        public int SellerStateID {get;set;}
        public bool SellerIsResident {get;set;}
        public string SellerPremises {get;set;}
        public string SellerAddressLine1 {get;set;}
        public string SellerAddressLine2 {get;set;}
        public string SellerPinCode {get;set;}
        public string SellerCity {get;set;}
        public string SellerEmailID {get;set;}
        public int PropertyType {get;set;}
        public string PropertyAddressLine1 {get;set;}  
        public string PropertyAddressLine2 {get;set;}
        public string PropertyPinCode {get;set;}
        public string TdsInterestRate {get;set;}
        public string PropertyStateID {get;set;}
        public string PropertyCity {get;set;}
        public string GstTaxCode {get;set;}
        public string TdsTaxCode { get;set;}
        public bool IsSharedSeller { get; set; }
        public decimal LateFeePerDay { get; set; }
    }
}
