﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace Services.Models
{
    public partial class CarEpolicy
    {
        /// <summary>
        /// ขนาดรถ
        /// </summary>
        public string EngineSize { get; set; }
        /// <summary>
        /// เลขตัวถัง
        /// </summary>
        public string CarTankNo { get; set; }
        /// <summary>
        /// ทะเบียนรถ
        /// </summary>
        public string CarLicense { get; set; }
        /// <summary>
        /// จังหวัดทะเบียนรถ
        /// </summary>
        public string CarProvince { get; set; }
        /// <summary>
        /// เลขกรมธรรม์
        /// </summary>
        public string PolicyNo { get; set; }
        /// <summary>
        /// ยี่ห้อรถ
        /// </summary>
        public string Marque { get; set; }

        public string StartDate { get; set; }
        public string StartTime { get; set; }
        public string EndDate { get; set; }
        public string EndTime { get; set; }
        public string CarTypeId { get; set; }

    }
}