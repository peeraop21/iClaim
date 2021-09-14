using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using DataAccess.EFCore.DigitalClaimModels;
using DinkToPdf;
using DinkToPdf.Contracts;
using Microsoft.AspNetCore.Mvc;
using Services;
using Services.ViewModels;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Core.Controllers
{
    public  class vwApproval
    {
        public string AccNo { get; set; }
        public int VictimNo { get; set; }
        public int AppNo { get; set; }
        public double? SumMoney { get; set; }
        public string ClaimNo { get; set; }
        public string Injury { get; set; }

        public List<vwBillsData> BillsData { get; set; }
        public vwBankData BankData { get; set; }
        public vwVictim VictimData { get; set;}

    }
    public class vwBillsData
    {
        public int billNo { get; set; }
        public string bill_no { get; set; }
        public string selectHospital { get; set; }
        public string money { get; set; }
        public string hospitalized_date { get; set; }
    }
    public class vwBankData
    {
        public string accountName { get; set; }
        public string accountNumber { get; set; }
        public string accountBankName { get; set; }
    }
    public class vwVictim
    {
        public string AccNo { get; set; }
        public int VictimNo { get; set; }
        public string Prefix { get; set; }
        public string Fname { get; set; }
        public string Lname { get; set; }
        public string Sex { get; set; }
        public short? Age { get; set; }
        public string DrvSocNo { get; set; }
        public string HomeId { get; set; }
        public string Moo { get; set; }
        public string Soi { get; set; }
        public string Road { get; set; }
        public string Tumbol { get; set; }
        public string TumbolName { get; set; }
        public string District { get; set; }
        public string DistrictName { get; set; }
        public string Province { get; set; }
        public string ProvinceName { get; set; }
        public string Zipcode { get; set; }
        public string TelNo { get; set; }
    }


    [Route("api/[controller]")]
    [ApiController]
    public class ApprovalController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IApprovalService approvalService;
        private readonly IConverter converter;
        public ApprovalController(IApprovalService approvalService, IMapper _mapper, IConverter converter)
        {
            this.converter = converter;
            this.approvalService = approvalService;
            this._mapper = _mapper;
        }
        [HttpGet("{accNo}/{victimNo}/{rightsType}")]
        public async Task<IActionResult> GetApproval(string accNo, int victimNo, int rightsType)
        {
            return Ok(await approvalService.GetApproval(accNo.Replace("-", "/"), victimNo, rightsType));
        }

        [HttpGet("HosApproval/{accNo}/{victimNo}")]
        public async Task<IActionResult> GetHosApproval(string accNo, int victimNo)
        {
            return Ok(await approvalService.GetHosApprovalsAsync(accNo.Replace("-", "/"), victimNo));
        }

        [HttpGet("DocumentReceive/{accNo}/{victimNo}/{appNo}")]
        public async Task<IActionResult> GetDocumentReceive(string accNo, int victimNo, int appNo)
        {
            return Ok(await approvalService.GetHosDocumentReceiveAsync(accNo.Replace("-", "/"), victimNo, appNo));
        }

        // POST api/<ApprovalController>
        [HttpPost]
        public async Task<IActionResult> PostAsync([FromBody] vwApproval model)
        {
            var resultMapHosApproval = _mapper.Map<HosApproval>(model);
            var resultMapBank = _mapper.Map<InputBankViewModel>(model.BankData);
            var resultMapVictim = _mapper.Map<VictimtViewModel>(model.VictimData);
            var addToDb = await approvalService.AddAsync(resultMapHosApproval, resultMapBank, resultMapVictim);
            return Ok(new { status = "" });
        }

        [HttpGet("GetPDF")]
        public async Task<IActionResult> GetApprovalDetailPDF()
        {
            byte[] file = null;
            var globalSettings = new GlobalSettings
            {
                ColorMode = ColorMode.Color,
                Orientation = Orientation.Portrait,
                PaperSize = PaperKind.A4,
                Margins = new MarginSettings { Top= 0, Bottom = 0 ,Left = 0, Right = 0},
                DocumentTitle = "บต3",
                DPI = 300,
                ImageDPI = 300,
                Outline = false
            };

            string HtmlContent = string.Empty;
            HtmlContent = await GenBotoBody();

            var objectSettings = new ObjectSettings
            {
                PagesCount = true,
                HtmlContent = HtmlContent,
                WebSettings = { DefaultEncoding = "utf-8" },
                LoadSettings =
                {
                    DebugJavascript = true,
                    StopSlowScript = false
                }
            };
            var pdf = new HtmlToPdfDocument()
            {
                GlobalSettings = globalSettings,
                Objects = { objectSettings }
            };
            file = converter.Convert(pdf);
            return File(file, "application/pdf");
        }

        private async Task<string> GenBotoBody()
        {
            var template = System.IO.Directory.GetCurrentDirectory() + @"\Templates\Boto3_Template.html";
            using (StreamReader reader = new StreamReader(template))
            {
                var htmlTemplate = await reader.ReadToEndAsync();
                return htmlTemplate;
            }
        }


    }
}
