using DataAccess.EFCore.iPolicyModels;
using DataAccess.EFCore.AccidentModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.ViewModels
{
    
    public class UpdateBankViewModel
    {
        public string accountName { get; set; }
        public string accountNumber { get; set; }
        public string accountBankName { get; set; }
        public string bankId { get; set; }
        public string bankBase64String { get; set; }
        public string bankFilename { get; set; }
        public bool isEditBankImage { get; set; }
    }
}
