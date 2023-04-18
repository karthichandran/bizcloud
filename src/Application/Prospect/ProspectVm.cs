using System;
using System.Collections.Generic;
using System.Text;

namespace ReProServices.Application.Prospect
{
    public class ProspectVm
    {
         public ICollection<ProspectDto> ProspectDto { get; set; }
        public ProspectPropertyDto ProspectPropertyDto { get; set; }
    }
}
