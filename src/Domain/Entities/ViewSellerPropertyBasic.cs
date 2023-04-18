using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ReProServices.Domain.Entities
{
    [Table("ViewSellerPropertyBasic")]
    public class ViewSellerPropertyBasic
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
        public string PropertyShortName { get; set; }
    }
}
