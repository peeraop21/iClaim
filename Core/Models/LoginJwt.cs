using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Core.Models
{
    public class LoginJwt
    {
        public string SystemName { get; set; }
        public string Password { get; set; }
        public string UserId { get; set; }
        public bool IsAccess { get; set; }
    }
}
