using DataAccess.EFCore.iPolicyModels;
using DataAccess.EFCore.AccidentModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.ViewModels
{
    
    public class UpdateInvoiceViewModel
    {
        public int billNo { get; set; }
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
        public string billFileShow { get; set; }
        public string filename { get; set; }
        public string editBillImage { get; set; }
        public bool isEditImage { get; set; }
        public bool isCancel { get; set; }
    }
}
