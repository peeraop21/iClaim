using DataAccess.EFCore.iPolicyModels;
using DataAccess.EFCore.AccidentModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.ViewModels
{
    
    public class ApprovalregisViewModel
    {
        
        /// <summary>
        /// เลขเคลม
        /// </summary>
        public string CrClaimno { get; set; }
        public string StringCrClaimno { get; set; }
        /// <summary>
        /// ลำดับผู้ประสบภัย
        /// </summary>
        public short VVictimno { get; set; }
        /// <summary>
        /// ลำดับคำร้อง
        /// </summary>
        public short ApRegno { get; set; }
        /// <summary>
        /// วันที่ยื่นคำร้อง
        /// </summary>
        public DateTime? ApRegdate { get; set; }
        public string StringApRegdate { get; set; }
        /// <summary>
        /// เลขที่ CV
        /// </summary>
        ///public string CvVoucherno { get; set; }
        /// <summary>
        /// วันที่บันทึก
        /// </summary>
        ///public DateTime? ApEntrydate { get; set; }
        /// <summary>
        /// ประเภทการจ่าย F=Full ,P=Partial
        /// </summary>
        ///public string ApRegtype { get; set; }
        /// <summary>
        /// การจ่าย
        /// </summary>
        public string ApPaytype { get; set; }
        /// <summary>
        /// จำนวนเงิน
        /// </summary>
        public decimal? ApMoney { get; set; }
        /// <summary>
        /// ผู้บันทึก
        /// </summary>
        ///public string ApEntryby { get; set; }
        /// <summary>
        /// คำนำหน้าผู้ยื่นคำร้อง
        /// </summary>
        ///public string ApRevprefix { get; set; }
        /// <summary>
        /// ชื่อผู้ยื่นคำร้อง
        /// </summary>
        ///public string ApRevfname { get; set; }
        /// <summary>
        /// นามสกุลผู้ยื่นคำร้อง
        /// </summary>
        ///public string ApRevlname { get; set; }
        ///public string ApRevfullname { get; set; }
        /// <summary>
        /// รหัสความสัมพันธ์ผู้ยื่นคำร้อง
        /// </summary>
        ///public string ApRevrelate { get; set; }
        /// <summary>
        /// ผู้อนุมัติ
        /// </summary>
        ///public string ApApproveby { get; set; }
        /// <summary>
        /// วันที่อนุมัติ
        /// </summary>
        ///public DateTime? ApApprovedate { get; set; }
        /// <summary>
        /// H=Hospital, V=Victim
        /// </summary>
        ///public string ApVtmhos { get; set; }
        ///public DateTime? ApDuedate { get; set; }
        /// <summary>
        /// รหัสความสัมพันธ์ผู้รับเงิน
        /// </summary>
        ///public string ApRecrelate { get; set; }
        /// <summary>
        /// คำนำหน้าผู้รับเงิน
        /// </summary>
        ///public string ApRecprefix { get; set; }
        /// <summary>
        /// ชื่อผู้รับเงิน
        /// </summary>
        ///public string ApRecfname { get; set; }
        /// <summary>
        /// นามสกุลผู้รับเงิน
        /// </summary>
        ///public string ApReclname { get; set; }
        /// <summary>
        /// รหัสจังหวัดรพ.ที่รับเงิน
        /// </summary>
        ///public string ApHosidprov { get; set; }
        /// <summary>
        /// รหัสรพ.ที่รับเงิน
        /// </summary>
        ///public string ApHosid { get; set; }
        /// <summary>
        /// เลขประจำตัวผู้รับเงิน
        /// </summary>
        ////public string ApRecid { get; set; }
        ////public string ApRecidprov { get; set; }
        ////public string ApReciddist { get; set; }
        ///public string ApInvoice { get; set; }
        /// <summary>
        /// วันที่จ่าย
        /// </summary>
        ///public DateTime? ApMoneydate { get; set; }
        /// <summary>
        /// เบื้องต้น ส่วนเกิน N=เบื้องต้น Y=ส่วนเกิน B=เบื้องต้นและส่วนเกิน
        /// </summary>
        ///public string ApPaymore { get; set; }
        /// <summary>
        /// สาขาที่จ่าย
        /// </summary>
        ///public string ApTransbranch { get; set; }
        ///public string ApSendstatus { get; set; }
        /// <summary>
        /// วันที่ปรับปรุงข้อมูลล่าสุด
        /// </summary>
        /*public DateTime? ApLastupdate { get; set; }
        public DateTime? ApSenddate { get; set; }
        public string ApTid { get; set; }
        public DateTime? ApTiddate { get; set; }
        public string ApTranstype { get; set; }*/
        public string ApStatus { get; set; }
       // public string ApMorecode { get; set; }
        /// <summary>
        /// การยืนยันความคุ้มครอง KCL014
        /// </summary>
        //public string ApCf014 { get; set; }
        /// <summary>
        /// วันที่ยืนยันความคุ้มครอง KCL014
        /// </summary>
        /*public DateTime? ApCf014Date { get; set; }
        public string ApMoreapproveby { get; set; }
        public DateTime? ApMoreapprovedate { get; set; }
        public string ApMoreapproveid { get; set; }*/
        /// <summary>
        /// ผู้ยืนยันความคุ้มครอง KCL014
        /// </summary>
       /* public string ApCf014user { get; set; }
        public string ApSummaryFlag { get; set; }
        public string ApDataStatus { get; set; }
        public bool? ApRcIoc { get; set; }
        public string ApTidSend { get; set; }
        public string ApConcat { get; set; }
        public DateTime? ApInsertDate { get; set; }
        public DateTime? ApUpdateDate { get; set; }
        public string ApSummaryPaid { get; set; }
        public DateTime? ApClearDate { get; set; }
        public string ApClearBy { get; set; }*/
        /// <summary>
        /// เลขรับแจ้ง
        /// </summary>
        public string AccNo { get; set; }
        public int AccAppNo { get; set; }
        public int AccVictimNo { get; set; }
        /// <summary>
        /// หมายเหตุต่างๆ เช่นการเรียกร้อง
        /// </summary>
        /*public string Comment { get; set; }
        public string RecoverySpecialStatus { get; set; }*/
        /// <summary>
        /// ลำดับผู้ประสบภัยของรับแจ้ง
        /// </summary>
        // public int? AccVictimNo { get; set; }
        /// <summary>
        /// ลำดับคำร้องของรับแจ้ง
        /// </summary>
        //public int? AccAppNo { get; set; }
        /// <summary>
        /// จำนวนเงินที่IOCยืนยันให้จ่าย
        /// </summary>
        //public double? ApCf014Money { get; set; }
        /// <summary>
        /// จำนวนเงินรวม
        /// </summary>
        public decimal? ApTotal { get; set; }
        /// <summary>
        /// ส่วนลด
        /// </summary>
        //public decimal? ApDiscount { get; set; }
        //public string ApRvpInvoice { get; set; }
        /// <summary>
        /// หมายเหตุการยืนยัน KCL014
        /// </summary>
       // public string ApCf014Comment { get; set; }
        /// <summary>
        /// หมายเหตุการบันทึกรับเงินคืน
        /// </summary>
        //public string ApPayBackComment { get; set; }
        //public string HospitalidTreat { get; set; }
        /// <summary>
        /// เลขที่ใบเสร็จ
        /// </summary>
        public string DailyReceiveno { get; set; }
        //public string CvpaidType { get; set; }
        //public string RecInsId { get; set; }
        //public string RegisbyBranch { get; set; }
        /// <summary>
        /// บต.
        /// </summary>
        public string Pt4 { get; set; }
        public string StringPt4 { get; set; }
        public string SubPt4 { get; set; }
        /// <summary>
        /// เลขที่ใบแจ้งหนี้
        /// </summary>
        //public string InvoiceEclaim { get; set; }
        /// <summary>
        /// ประเภทบัตร
        /// </summary>
        //public string MCardType { get; set; }
        /// <summary>
        /// ประเภทบัตรอื่นๆ
        /// </summary>
        //public string MCardTypeOther { get; set; }
        // public short? ApRecnationalityId { get; set; }
        public object Claim { get; set; }
        public double? CureMoneyAmount { get; set; }
        public double? CrippledMoneyAmount { get; set; }
        public object PtDetail { get; set; }


    }
}
