using Core.ViewModels;
using System.Threading.Tasks;
using AutoMapper;
using DataAccess.EFCore.DigitalClaimModels;
using DinkToPdf;
using DinkToPdf.Contracts;
using Microsoft.AspNetCore.Mvc;
using Services;
using Services.ViewModels;
using System;
using System.Globalization;
using System.Collections.Generic;


// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Core.Controllers
{
    


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
        public async Task<IActionResult> PostAsync([FromBody] ApprovalViewModel model)
        {
            var resultMapHosApproval = _mapper.Map<HosApproval>(model);
            var resultMapBank = _mapper.Map<InputBankViewModel>(model.BankData);
            var resultMapVictim = _mapper.Map<VictimtViewModel>(model.VictimData);
            var resultMapToInvoicehd = _mapper.Map<Invoicehd[]>(model.BillsData);
            
            var addToDb = await approvalService.AddAsync(resultMapHosApproval, resultMapBank, resultMapVictim, resultMapToInvoicehd);
            return Ok(new { status = "" });
        }

        [HttpGet("UpdateStatus/{accNo}/{victimNo}/{appNo}/{status}")]
        public async Task<IActionResult> UpdateStatus(string accNo, int victimNo, int appNo, string status)
        {
            return Ok(await approvalService.UpdateApprovalAsync(accNo.Replace('-', '/'), victimNo, appNo, status));
        }

        [HttpGet("LastDocumentReceive/{accNo}/{victimNo}")]
        public async Task<IActionResult> GetLastDocumentReceive(string accNo, int victimNo)
        {
            return Ok(await approvalService.GetLastHosDocumentReceiveAsync(accNo.Replace("-", "/"), victimNo));
        }



    }
}
