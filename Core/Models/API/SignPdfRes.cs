using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Core.Models.API
{
    public class SignPdfRes
    {
        public int Status { get; set; }
        public string Message { get; set; }
        public string Base64Pdf { get; set; }
    }
}
