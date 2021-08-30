using DataAccess.EFCore.iPolicyModels;
using DataAccess.EFCore.AccidentModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.ViewModels
{
    
    public class CarViewModel
    {

        public string FoundCarLicense { get; set; }
        public string FoundChassisNo { get; set; }
        public string FoundPolicyNo { get; set; }
    }
}
