using System;
using System.Collections.Generic;
using System.Text;

namespace Services.ViewModels
{
    public class ECMViewModel
    {
        public string RefNo { get; set; }
        public string SystemId { get; set; }
        public string TemplateId { get; set; }
        public string DocID { get; set; }
        public string FileName { get; set; }
        public string Base64String { get; set; }
        public string Path { get; set; }

    }
    public class ECMViewModelRes
    {
        public string Path { get; set; }
        public string DocumentID { get; set; }

    }
}
