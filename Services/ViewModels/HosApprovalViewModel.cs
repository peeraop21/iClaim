﻿using DataAccess.EFCore.iPolicyModels;
using DataAccess.EFCore.AccidentModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.ViewModels
{
    
    public class HosApprovalViewModel
    {

        public string AccNo { get; set; }

        public string StringAccNo { get; set; }
        public int VictimNo { get; set; }
        public int AppNo { get; set; }
        public DateTime? DateCreate { get; set; }
        public DateTime? RegDate { get; set; }
        public string StringRegDate { get; set; }
        public string RegType { get; set; }
        public double? MedicineMoney { get; set; }
        public double? PlasticMoney { get; set; }
        public double? ServiceMoney { get; set; }
        public double? RoomMoney { get; set; }
        public double? VeihcleMoney { get; set; }
        public double? SumMoney { get; set; }
        public string Pt4id { get; set; }
        public string InvoiceNo { get; set; }
        public string InvoiceId { get; set; }
        public string HosId { get; set; }
        public DateTime? LastUpdate { get; set; }
        public string ClaimNo { get; set; }
        public short? VictimNoClaim { get; set; }
        public short? RegNoClaim { get; set; }
        public string DocumentNo { get; set; }
        public string BranchId { get; set; }
        public string GetRecordStatus { get; set; }
        public string UserId { get; set; }
        public string PayMore { get; set; }
        public double? CureMoney { get; set; }
        public double? DeadMoney { get; set; }
        public double? HygieneMoney { get; set; }
        public double? CrippledMoney { get; set; }
        public string RevPrefix { get; set; }
        public string RevFname { get; set; }
        public string RevLname { get; set; }
        public string RevRelate { get; set; }
        public string RecPrefix { get; set; }
        public string RecFname { get; set; }
        public string RecLname { get; set; }
        public string RecRelate { get; set; }
        public string RecSocNo { get; set; }
        public string AcceptBy { get; set; }
        public DateTime? AcceptDate { get; set; }
        public string AppStatusName { get; set; }
        public List<ApprovalStatusViewModel> AppStatus { get; set; }
    }
}
