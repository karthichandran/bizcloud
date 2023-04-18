using System.ComponentModel.DataAnnotations;

namespace ReProServices.Domain.Entities
{
    public class StatusType
    {
        [Key]
        public int StatusTypeID { get; set; }
        public string Status { get; set; }

    }
}
