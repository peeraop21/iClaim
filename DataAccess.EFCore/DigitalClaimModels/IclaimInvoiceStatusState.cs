﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using System;
using System.Collections.Generic;

namespace DataAccess.EFCore.DigitalClaimModels
{
    public partial class IclaimInvoiceStatusState
    {
        public long IdInvhd { get; set; }
        public string AccNo { get; set; }
        public int? VictimNo { get; set; }
        public int? ReqNo { get; set; }
        public int StateNo { get; set; }
        public int? OldStatus { get; set; }
        public int? NewStatus { get; set; }
        public DateTime? InsertDate { get; set; }
        public string InvDocComment { get; set; }
        public string RecordBy { get; set; }
    }
}