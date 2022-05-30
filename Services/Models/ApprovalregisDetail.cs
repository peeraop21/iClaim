using DataAccess.EFCore.iPolicyModels;
using DataAccess.EFCore.AccidentModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Models
{
    
    public class ApprovalregisDetail
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
        /// การจ่าย
        /// </summary>
        public string ApPaytype { get; set; }
        /// <summary>
        /// จำนวนเงิน
        /// </summary>
        public decimal? ApMoney { get; set; }
        public string ApStatus { get; set; }
        /// <summary>
        /// เลขรับแจ้ง
        /// </summary>
        public string AccNo { get; set; }
        public int AccAppNo { get; set; }
        public int AccVictimNo { get; set; }
        /// <summary>
        /// จำนวนเงินรวม
        /// </summary>
        public decimal? ApTotal { get; set; }
        /// <summary>
        /// เลขที่ใบเสร็จ
        /// </summary>
        public string DailyReceiveno { get; set; }
        /// <summary>
        /// บต.
        /// </summary>
        public string Pt4 { get; set; }
        public string StringPt4 { get; set; }
        public string SubPt4 { get; set; }
        public object Claim { get; set; }
        public double? CureMoneyAmount { get; set; }
        public double? CrippledMoneyAmount { get; set; }
        public object PtDetail { get; set; }


    }
}
