using System.ComponentModel.DataAnnotations;

namespace ReProServices.Domain.Entities
{
    public class RemittanceStatus
    {
        [Key]
        public int RemittanceStatusID { get; set; }
        public string RemittanceStatusText { get; set; }

    }
}
