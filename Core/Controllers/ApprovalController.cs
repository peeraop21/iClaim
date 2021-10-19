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
        private readonly IAttachmentService attachmentService;
        public ApprovalController(IApprovalService approvalService, IMapper _mapper, IConverter converter, IAttachmentService attachmentService)
        {
            this.converter = converter;
            this.approvalService = approvalService;
            this._mapper = _mapper;
            this.attachmentService = attachmentService;
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

            var result = await approvalService.AddAsync(resultMapHosApproval, resultMapBank, resultMapVictim, resultMapToInvoicehd);
            ECMViewModel ecmModel = new ECMViewModel();
            
            for (int i = 0; i < model.BillsData.Count; i++)
            {
                ecmModel.RefNo = result[i].IdInvhd + "|" + result[0].AccNo + "|" + result[0].VictimNo + "|" + result[0].AppNo;
                ecmModel.SystemId = "02";
                ecmModel.TemplateId = "03";
                ecmModel.DocID = "01";               
                ecmModel.FileName = model.BillsData[i].filename;
                ecmModel.Base64String = model.BillsData[i].billFileShow;
                var invoiceEcmRes = await attachmentService.UploadFileToECM(ecmModel);
                var resultMapEdocDetail = _mapper.Map<EdocDetailViewModel>(ecmModel);
                resultMapEdocDetail.Paths = invoiceEcmRes.Path;
                await attachmentService.SaveToEdocDetail(resultMapEdocDetail);
            }
            ecmModel.RefNo = result[0].AccNo + "|" + result[0].VictimNo + "|" + result[0].AppNo;
            ecmModel.SystemId = "03";
            ecmModel.TemplateId = "09";
            ecmModel.DocID = "01";
            ecmModel.FileName = model.BankData.bankFilename;
            ecmModel.Base64String = model.BankData.bankBase64String;
            var bookbankEcmRes = await attachmentService.UploadFileToECM(ecmModel);
            var bookbankResultMapEdocDetail = _mapper.Map<EdocDetailViewModel>(ecmModel);
            bookbankResultMapEdocDetail.Paths = bookbankEcmRes.Path;
            await attachmentService.SaveToEdocDetail(bookbankResultMapEdocDetail);

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
        [HttpGet("Invoicehd/{accNo}/{victimNo}/{appNo}")]
        public async Task<IActionResult> GetInvoicehd(string accNo, int victimNo, int appNo)
        {
            return Ok(await approvalService.GetInvoicehdAsync(accNo.Replace("-", "/"), victimNo,appNo));
        }

        [HttpGet("DocumentCheck/{accNo}/{victimNo}/{appNo}")]
        public async Task<IActionResult> GetDocumentCheck(string accNo, int victimNo, int appNo)
        {
            return Ok(await approvalService.GetDocumentCheck(accNo.Replace("-", "/"), victimNo, appNo));
        }

        //[HttpGet("TestGenPt/{accNo}/{victimNo}/{appNo}")]
        //public async Task<IActionResult> TestGenPt(string accNo, int victimNo, int appNo)
        //{

        //    return Ok(await approvalService.GeneratePT(accNo.Replace("-", "/"), victimNo, appNo));
        //}
        [HttpPost("DownloadFromECM")]
        public async Task<IActionResult> GetBase64FromECM([FromBody] EdocDetailViewModel model)
        {
            var documentPath = await attachmentService.GetDocumentPath(model);            
            return Ok(await attachmentService.DownloadFileFromECM(documentPath));
        }

    }
}
