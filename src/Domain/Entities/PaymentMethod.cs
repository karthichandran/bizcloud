using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ReProServices.Domain.Entities
{
    public class PaymentMethod
    {
        [Key]
        [Column("PaymentMethodID")]
        public int PaymentMethodId { get; set; }
        [Column("PaymentMethod")]
        [StringLength(15)]
        public string PaymentMethod1 { get; set; }

        [InverseProperty("PaymentMethodNavigation")]
        public virtual ICollection<CustomerProperty> CustomerProperty { get; set; } = new HashSet<CustomerProperty>();
    }
}
