using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using DataAccess.EFCore.DigitalClaimModels;
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
        public ApprovalController(IApprovalService approvalService, IMapper _mapper)
        {
            this.approvalService = approvalService;
            this._mapper = _mapper;
        }
        [HttpGet("{accNo}/{rightsType}")]
        public async Task<IActionResult> GetApproval(string accNo, int rightsType)
        {
            return Ok(await approvalService.GetApproval(accNo.Replace("-", "/"), rightsType));
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
        public async Task<IActionResult> PostAsync([FromBody]vwApproval model)
        {
            var resultMapHosApproval = _mapper.Map<HosApproval>(model);
            var resultMapBank = _mapper.Map<InputBankViewModel>(model.BankData);
            var resultMapVictim = _mapper.Map<VictimtViewModel>(model.VictimData);
            var addToDb  = await approvalService.AddAsync(resultMapHosApproval, resultMapBank, resultMapVictim);
            return Ok(new { status = "" });
        }

        



    }
}
