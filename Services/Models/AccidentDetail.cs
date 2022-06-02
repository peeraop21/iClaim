using DataAccess.EFCore.iPolicyModels;
using DataAccess.EFCore.AccidentModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Models
{
    
    public class AccidentDetail
    {
        public string AccNo { get; set; }
        public DateTime? DateAcc { get; set; }
        public string DateAccString { get; set; }
        public string TimeAcc { get; set; }
        public string AccPlace { get; set; }
        public string AccProv { get; set; }
        public string AccDist { get; set; }
        public string AccSubDist { get; set; }
        public string AccNature { get; set; }
    }
}
