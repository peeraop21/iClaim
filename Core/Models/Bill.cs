

using System.Collections.Generic;

namespace Core.Models
{

    public class Bill
    {
        public int billNo { get; set; }
        public string accNo { get; set; }
        public int victimNo { get; set; }
        public int reqNo { get; set; }
        public string bill_no { get; set; }
        public string bookNo { get; set; }
        public string injuri { get; set; }
        public short injuriId { get; set; }
        public string typePatient { get; set; }
        public string selectHospital { get; set; }
        public string selectHospitalId { get; set; }
        public string money { get; set; }
        public string hospitalized_date { get; set; }
        public string out_hospital_date { get; set; }
        public string hospitalized_time { get; set; }
        public string out_hospital_time { get; set; }
        public List<string> billFileShow { get; set; }
        public List<string> filename { get; set; }
        public List<string> editBillImage { get; set; }
        public bool isEditImage { get; set; }
        public bool isCancel { get; set; }

    }
}
