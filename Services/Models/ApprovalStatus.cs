using DataAccess.EFCore.iPolicyModels;
using DataAccess.EFCore.AccidentModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Models
{
    
    public class ApprovalStatus
    {
        public int StatusId { get; set; }
        public string StatusName { get; set; }
        public bool Active { get; set; }
        public string StatusDate { get; set; }
        public string StatusTime { get; set; }
    }
}
