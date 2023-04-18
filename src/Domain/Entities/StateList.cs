using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ReProServices.Domain.Entities
{
    public class StateList
    {
        public StateList()
        {
            Customer = new HashSet<Customer>();
            Property = new HashSet<Property>();
            Seller = new HashSet<Seller>();
        }

        [Key]
        [Column("StateID")]
        public int StateId { get; set; }
        [Required]
        [StringLength(32)]
        public string State { get; set; }
        [Required]
        [StringLength(22)]
        public string Abbreviation { get; set; }

        [InverseProperty("StateIdFkNavigation")]
        public virtual ICollection<Customer> Customer { get; set; }
        [InverseProperty("State")]
        public virtual ICollection<Property> Property { get; set; }
        [InverseProperty("State")]
        public virtual ICollection<Seller> Seller { get; set; }
    }
}
