using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Core.Models.API
{
    public class SignPdfReq
    {
        public byte[] PdfBytes { get; set; }
        public string AccNo { get; set; }
        public int VictimNo { get; set; }
        public int ReqNo { get; set; }
    }
}
