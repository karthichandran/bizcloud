using System.ComponentModel.DataAnnotations;

namespace ReProServices.Domain.Entities
{
    public class NatureOfPayment
    {
        [Key]
        public int NatureOfPaymentID { get; set; }
        public string NatureOfPaymentText { get; set; }
    }
}
