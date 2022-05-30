using System;
using System.Collections.Generic;
using System.Text;

namespace Services.Models
{
    public class ResultWound
    {
        public List<Wound> WoundedList { get; set; }
        public List<string> Organ { get; set; }
    }
    public class Wound
    {
        public short WoundedId { get; set; }
        public string Wounded { get; set; }

        public string Organ { get; set; }
    }
}
