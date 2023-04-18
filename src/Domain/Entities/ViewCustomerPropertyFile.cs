using System;
using System.ComponentModel.DataAnnotations;

namespace ReProServices.Domain.Entities
{
    public class ViewCustomerPropertyFile
    {
        [Key]
        public int BlobID { get; set; }
        public Guid? OwnershipID { get; set; }
        public string FileName { get; set; }
        public DateTime? UploadTime { get; set; }
        public string PanID { get; set; }
        public int FileCategoryId { get; set; }
    }
}
