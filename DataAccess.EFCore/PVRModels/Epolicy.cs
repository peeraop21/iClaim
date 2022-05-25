﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace DataAccess.EFCore.PVRModels
{
    public partial class Epolicy
    {
        /// <summary>
        /// ลำดับ
        /// </summary>
        public int Seq { get; set; }
        /// <summary>
        /// วันที่ขาย
        /// </summary>
        public DateTime? Policydate { get; set; }
        /// <summary>
        /// ชื่อผู้ใช้
        /// </summary>
        public string Userid { get; set; }
        public string Kum025id { get; set; }
        /// <summary>
        /// รหัสสาขา
        /// </summary>
        public string Branchid { get; set; }
        /// <summary>
        /// รหัสภาค
        /// </summary>
        public string Regionid { get; set; }
        /// <summary>
        /// รหัสตัวแทน
        /// </summary>
        public string Agentid { get; set; }
        /// <summary>
        /// ชื่อ
        /// </summary>
        public string Firstname { get; set; }
        /// <summary>
        /// สกุล
        /// </summary>
        public string Surname { get; set; }
        /// <summary>
        /// ที่อยู่
        /// </summary>
        public string Addr { get; set; }
        /// <summary>
        /// เบอร์โทร
        /// </summary>
        public string Telephoneno { get; set; }
        public string Hmo { get; set; }
        /// <summary>
        /// คำนำหน้า
        /// </summary>
        public string Titlename { get; set; }
        /// <summary>
        /// ถนน
        /// </summary>
        public string Road { get; set; }
        /// <summary>
        /// ตำบล
        /// </summary>
        public string Ptumbol { get; set; }
        /// <summary>
        /// อำเภอ
        /// </summary>
        public string Pamphur { get; set; }
        /// <summary>
        /// จังหวัด
        /// </summary>
        public string Pchangwat { get; set; }
        /// <summary>
        /// รหัสไปรษณีย์
        /// </summary>
        public string Pzipcode { get; set; }
        /// <summary>
        /// ขนาดรถ
        /// </summary>
        public string Enginesize { get; set; }
        /// <summary>
        /// เบี้ยประกัน
        /// </summary>
        public double? Ppremium { get; set; }
        /// <summary>
        /// อากร
        /// </summary>
        public short? Pstamp { get; set; }
        /// <summary>
        /// ภาษี
        /// </summary>
        public double? Pvat { get; set; }
        /// <summary>
        /// รวมเงิน
        /// </summary>
        public double? Ptotal { get; set; }
        public double? Pvatrate { get; set; }
        /// <summary>
        /// เลขตัวถัง
        /// </summary>
        public string Cartankno { get; set; }
        /// <summary>
        /// ทะเบียนรถ
        /// </summary>
        public string Carno { get; set; }
        /// <summary>
        /// จังหวัดทะเบียนรถ
        /// </summary>
        public string Carchangwat { get; set; }
        /// <summary>
        /// เลขกรมธรรม์
        /// </summary>
        public string Policyno { get; set; }
        /// <summary>
        /// วันที่คุ้มครอง
        /// </summary>
        public DateTime? Startdate { get; set; }
        /// <summary>
        /// เวลาเริ่มคุ้มครอง
        /// </summary>
        public string Starttime { get; set; }
        /// <summary>
        /// วันที่สิ้นสุดความคุ้มครอง
        /// </summary>
        public DateTime? Enddate { get; set; }
        /// <summary>
        /// สถานะ
        /// </summary>
        public string Status { get; set; }
        public DateTime? Stampdate { get; set; }
        public DateTime? Vatdate { get; set; }
        /// <summary>
        /// วันที่บันทึก
        /// </summary>
        public DateTime? Keydate { get; set; }
        /// <summary>
        /// เวลาที่บันทึก
        /// </summary>
        public string Keytime { get; set; }
        public DateTime? Stickerreceivedate { get; set; }
        public DateTime? Statusdate { get; set; }
        /// <summary>
        /// รหัสรถ
        /// </summary>
        public string Pcartype { get; set; }
        public string Kum038 { get; set; }
        public string Sflg { get; set; }
        public string Kum038serial { get; set; }
        /// <summary>
        /// ยี่ห้อรถ
        /// </summary>
        public string Marque { get; set; }
        public string Receiptno { get; set; }
        /// <summary>
        /// วันที่ปิดงวด
        /// </summary>
        public DateTime? Closeperiod { get; set; }
        public string Apno { get; set; }
        /// <summary>
        /// วันที่ยกเลิก
        /// </summary>
        public DateTime? Canceldate { get; set; }
        public string Canceltype { get; set; }
        public double? Returnpremium { get; set; }
        /// <summary>
        /// ค่าคอมมิชชัน
        /// </summary>
        public double? Commission { get; set; }
        /// <summary>
        /// จำนวนวันที่คุ้มครอง
        /// </summary>
        public int? Activedays { get; set; }
        public string Oldstickerid { get; set; }
        public string Ctype { get; set; }
        public string Stickerid { get; set; }
        /// <summary>
        /// รหัสบัตรปชช
        /// </summary>
        public string Idcard { get; set; }
        public double? Linecommission { get; set; }
        public string Lineagent { get; set; }
        public decimal? Wht { get; set; }
        public decimal? Vat { get; set; }
        public decimal? Repay { get; set; }
        public string Carmodel { get; set; }
        public string Carcolor { get; set; }
    }
}