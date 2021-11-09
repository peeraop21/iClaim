using System;
using System.Collections.Generic;
using System.Text;

namespace Services.ViewModels
{
    public class InvoicehdViewModel
    {
        public long IdInvhd { get; set; }
        public string AccNo { get; set; }
        public int? VictimNo { get; set; }
        public int? AppNo { get; set; }
        public string Hosid { get; set; }
        public string Victimname { get; set; }
        public int? Age { get; set; }
        public string An { get; set; }
        public string Hn { get; set; }
        public DateTime? Takendate { get; set; }
        public string Takentime { get; set; }
        public DateTime? Dispensedate { get; set; }
        public string Dispensetime { get; set; }
        public int? Daybed { get; set; }
        public string Mainconsider { get; set; }
        public string Consider { get; set; }
        public string Maincomment { get; set; }
        public string Comment { get; set; }
        public string Vname { get; set; }
        public string Hostype { get; set; }
        public double? Suminv { get; set; }
        public long? IdInvdt { get; set; }
        public string InvoiceNo { get; set; }
        public DateTime? InvoiceDate { get; set; }
        public string InputBy { get; set; }
        public string RecordBy { get; set; }
        public DateTime? RecordDate { get; set; }
        public string InvoiceType { get; set; }
        public short? IsApply { get; set; }
        public string BookNo { get; set; }
        public string ReceiptNo { get; set; }
        public DateTime? RecDate { get; set; }
        public string VictimType { get; set; }
        public DateTime? ReceiptDate { get; set; }
        public string InvoiceStatus { get; set; }
        public string WoundedName { get; set; }
        public string HospitalName { get; set; }
        public string StringTakendate { get; set; }
        public string StringDispensedate { get; set; }
        public string Base64Image { get; set; }
    }
}
