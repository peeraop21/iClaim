using System;
using System.Collections.Generic;
using System.Text;

namespace Services.Models
{
    public class DocumentDetailRes
    {
        public string Path { get; set; }
        public string DocumentID { get; set; }
        public string Message { get; set; }
        public string Status { get; set; }
    }
    public class DocumentDetail
    {
        public string SystemId { get; set; }
        public string TemplateId { get; set; }
        public string DocumentId { get; set; }
        public string RefId { get; set; }
        public int? RunningNo { get; set; }
        public string Paths { get; set; }
        public string StatusDoc { get; set; }
        public DateTime? CreateDate { get; set; }
        public string Createby { get; set; }
        public string CreateIp { get; set; }
        public DateTime? CancelDate { get; set; }
        public string Cancelby { get; set; }
        public string CancelIp { get; set; }


    }

    
}
