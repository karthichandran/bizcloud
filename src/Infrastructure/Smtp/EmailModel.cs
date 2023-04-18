using System;
using System.Collections.Generic;
using System.Text;

namespace ReProServices.Infrastructure.Smtp
{
    public class EmailModel
    {
        public string From { get; set; }
        public string To { get; set; }
        public string CC { get; set; }
        public string Subject { get; set; }
        public string Message { get; set; }
        public bool IsBodyHtml { get; set; }   
        public ICollection<FileAttachment> attachments { get; set; }

        public byte[] FileBlob { get; set; }
    }

    public class FileAttachment {
        public byte[] MemoryStream { get; set; }
        public string FileName { get; set;}
        public string FileType { get; set; }
    }
}
