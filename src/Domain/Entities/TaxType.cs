using System.ComponentModel.DataAnnotations;

namespace ReProServices.Domain.Entities
{
    public class TaxType
    {
        [Key]
        public int TaxTypeId { get; set; }
        public string TaxTypeDesc { get; set; }

    }
}
