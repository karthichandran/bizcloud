using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace ReProServices.Application.DebitAdvices
{
   public class DebitAdviceWithFileDto :DebitAdviceDto
    {
        public MultipartFormDataContent Files { get; set; }
    }
}
