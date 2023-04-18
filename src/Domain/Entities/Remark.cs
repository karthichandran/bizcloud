using System.ComponentModel.DataAnnotations;

namespace ReProServices.Domain.Entities
{
    public class Remark
    {
        [Key]
        public int RemarksID { get; set; }
        public string RemarksText { get; set; }
    }
}
