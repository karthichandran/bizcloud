using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ReProServices.Domain.Entities
{
    public class Segment
    {
        [Key]
        public int SegmentID{get;set;}
        public string Name { get; set; }
    }
}
