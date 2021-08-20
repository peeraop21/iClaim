using DataAccess.EFCore.BankNamesModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.ViewModels
{

    public class BankNamesViewModel
    {
        public string Bank { get; set; }
        public string Name { get; set; }
        public string Default0 { get; set; }
        public string BankCode { get; set; }

    }
}