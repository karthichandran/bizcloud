using System.ComponentModel.DataAnnotations;

namespace ReProServices.Domain.Entities
{
    public class FileCategory
    {
        [Key]
        public int FileCategoryId { get; set; }
        public string FileCategoryName { get; set; }

    }
}
