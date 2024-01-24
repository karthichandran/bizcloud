using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ReProServices.Domain.Entities
{
    public class RemittanceRemark
    {
        [Key]
        public int RemarkId { get; set; }

        public string Description { get; set; }
        public bool IsRemittance { get; set; }
    }
}
