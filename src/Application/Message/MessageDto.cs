using System;
using System.Collections.Generic;
using System.Text;

namespace ReProServices.Application.Message
{
   public class MessageDto
    {
        public int MessageID { get; set; }

        public string Subject { get; set; }
        public string Body { get; set; }
        public bool? Verified { get; set; }
        public int? Lane { get; set; }
        public string Message { get; set; }
        public int? Error_code { get; set; }
        public int Opt { get; set; }
    }
}
