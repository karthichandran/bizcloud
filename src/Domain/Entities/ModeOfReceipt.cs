using System.ComponentModel.DataAnnotations;

namespace ReProServices.Domain.Entities
{
    public class ModeOfReceipt
    {
        [Key]
        public int ModeOfReceiptID { get; set; }
        public string ModeOfReceiptText { get; set; }

    }
}
