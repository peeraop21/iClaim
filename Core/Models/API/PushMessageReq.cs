using Newtonsoft.Json.Linq;
using System.Collections.Generic;


namespace Core.Models.API
{
    public class PushMessageReq
    {
        public string To { get; set; }      
        public string Messages { get; set; }
        public string Type { get; set; }

    }
}
