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
using DataAccess.EFCore.RvpOfficeModels;


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
            return Ok(await approvalService.GetApprovalRegis(accNo.Replace("-", "/"), victimNo, rightsType));
        }

        [HttpGet("HosApproval/{accNo}/{victimNo}")]
        public async Task<IActionResult> GetHosApproval(string accNo, int victimNo)
        {
            return Ok(await approvalService.GetIClaimApprovalAsync(accNo.Replace("-", "/"), victimNo));
        }

        [HttpGet("DocumentReceive/{accNo}/{victimNo}/{appNo}")]
        public async Task<IActionResult> GetDocumentReceive(string accNo, int victimNo, int appNo)
        {
            return Ok(await approvalService.GetIClaimBankAccountAsync(accNo.Replace("-", "/"), victimNo, appNo));
        }

        // POST api/<ApprovalController>
        [HttpPost]
        public async Task<IActionResult> PostAsync([FromBody] ApprovalViewModel model)
        {

            var resultMapIclaimApproval = _mapper.Map<IclaimApproval>(model);
            var resultMapBank = _mapper.Map<InputBankViewModel>(model.BankData);
            var resultMapVictim = _mapper.Map<VictimtViewModel>(model.VictimData);
            var resultMapToInvoicehd = _mapper.Map<Invoicehd[]>(model.BillsData);

            var result = await approvalService.AddAsync(resultMapIclaimApproval, resultMapBank, resultMapVictim, resultMapToInvoicehd, model.UserIdLine);
            ECMViewModel ecmModel = new ECMViewModel();
            
            for (int i = 0; i < model.BillsData.Count; i++)
            {
                
                //var refTemplate = result[i].IdInvhd + "|" + result[0].AccNo + "|" + result[0].VictimNo + "|" + result[0].AppNo;
                
                ecmModel.SystemId = "02";
                ecmModel.TemplateId = "03";
                ecmModel.DocID = "01";
                //var lastRefNo = await attachmentService.GetLastEdocDetailAsync(ecmModel.SystemId, ecmModel.TemplateId, ecmModel.DocID, refTemplate);
                //if (lastRefNo != null)
                //{
                //    int runNo = int.Parse(lastRefNo.Substring(lastRefNo.Length - 5)) + 1;
                //    var leadZeroRunNo = runNo.ToString().PadLeft(5, '0');
                //    ecmModel.RefNo = refTemplate + "-" + leadZeroRunNo;
                //}
                //else
                //{
                //    ecmModel.RefNo = refTemplate + "-" + "00001";
                //}
                ecmModel.RefNo = result[i].IdInvhd + "|" + result[0].AccNo + "|" + result[0].VictimNo /* + "|" + result[0].AppNo*/;
                ecmModel.FileName = DateTime.Now.ToString("ddMMyyyyHHmmss") + model.BillsData[i].filename;
                ecmModel.Base64String = model.BillsData[i].billFileShow;
                var invoiceEcmRes = await attachmentService.UploadFileToECM(ecmModel);
                var resultMapEdocDetail = _mapper.Map<EdocDetailViewModel>(ecmModel);
                resultMapEdocDetail.Paths = invoiceEcmRes.Path;
                await attachmentService.SaveToEdocDetail(resultMapEdocDetail);
            }
            //var refTemplateBank = result[0].AccNo + "|" + result[0].VictimNo + "|" + result[0].AppNo;            
            ecmModel.SystemId = "03";
            ecmModel.TemplateId = "09";
            ecmModel.DocID = "01";
            //var lastRefNoBank = await attachmentService.GetLastEdocDetailAsync(ecmModel.SystemId, ecmModel.TemplateId, ecmModel.DocID, refTemplateBank);
            //if (lastRefNoBank != null)
            //{
            //    int runNoBank = int.Parse(lastRefNoBank.Substring(lastRefNoBank.Length - 5)) + 1;
            //    var leadZeroRunNoBank = runNoBank.ToString().PadLeft(5, '0');
            //    ecmModel.RefNo =  refTemplateBank + "-" + leadZeroRunNoBank;
            //}
            //else
            //{
            //    ecmModel.RefNo = refTemplateBank + "-" + "00001";
            //}
            ecmModel.RefNo = result[0].IclaimAppNo + "|" + result[0].AccNo + "|" + result[0].VictimNo;
            ecmModel.FileName = (string.IsNullOrEmpty(model.BankData.bankFilename)) ? DateTime.Now.ToString("ddMMyyyyHHmmss") + "bankold.png" : DateTime.Now.ToString("ddMMyyyyHHmmss") + model.BankData.bankFilename;
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
            return Ok(await approvalService.UpdateApprovalStatusAsync(accNo.Replace('-', '/'), victimNo, appNo, status, false));
        }

        [HttpGet("LastDocumentReceive/{accNo}/{victimNo}")]
        public async Task<IActionResult> GetLastDocumentReceive(string accNo, int victimNo)
        {
            return Ok(await approvalService.GetLastIClaimBankAccountAsync(accNo.Replace("-", "/"), victimNo));
        }
        [HttpGet("Invoicehd/{accNo}/{victimNo}/{appNo}")]
        public async Task<IActionResult> GetInvoicehd(string accNo, int victimNo, int appNo)
        {
            return Ok(await approvalService.GetInvoicehdAsync(accNo.Replace("-", "/"), victimNo, appNo, 2));
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

        [HttpPost("UpdateApproval")]
        public async Task<IActionResult> UpdateApproval([FromBody] ApprovalViewModel model)
        {
            var resultMapBank = _mapper.Map<UpdateBankViewModel>(model.BankData);
            var resultMapToInvoicehd = _mapper.Map<UpdateInvoiceViewModel[]>(model.BillsData);
            var result = await approvalService.UpdateAsync(model.AccNo, model.VictimNo, model.AppNo, model.UserIdLine, resultMapBank, resultMapToInvoicehd);
            ECMViewModel ecmModel = new ECMViewModel();
            
            for (int i = 0; i < model.BillsData.Count; i++)
            {
                if (resultMapToInvoicehd[i].isEditImage && !resultMapToInvoicehd[i].isCancel)
                {
                    ecmModel.SystemId = "02";
                    ecmModel.TemplateId = "03";
                    ecmModel.DocID = "01";                   
                    ecmModel.RefNo = resultMapToInvoicehd[i].billNo + "|" + model.AccNo + "|" + model.VictimNo  /*+ "|" + model.AppNo*/;
                    ecmModel.FileName = DateTime.Now.ToString("ddMMyyyyHHmmss") + resultMapToInvoicehd[i].filename;
                    ecmModel.Base64String = resultMapToInvoicehd[i].editBillImage;
                    var invoiceEcmRes = await attachmentService.UploadFileToECM(ecmModel);
                    var resultMapEdocDetail = _mapper.Map<EdocDetailViewModel>(ecmModel);
                    resultMapEdocDetail.Paths = invoiceEcmRes.Path;
                    await attachmentService.SaveToEdocDetail(resultMapEdocDetail);
                }
                
            }
            if (resultMapBank.isEditBankImage)
            {
                ecmModel.RefNo = model.AppNo + "|" + model.AccNo + "|" + model.VictimNo;
                ecmModel.SystemId = "03";
                ecmModel.TemplateId = "09";
                ecmModel.DocID = "01";
                ecmModel.FileName = DateTime.Now.ToString("ddMMyyyyHHmmss") + model.BankData.bankFilename;
                ecmModel.Base64String = model.BankData.bankBase64String;
                var bookbankEcmRes = await attachmentService.UploadFileToECM(ecmModel);
                var bookbankResultMapEdocDetail = _mapper.Map<EdocDetailViewModel>(ecmModel);
                bookbankResultMapEdocDetail.Paths = bookbankEcmRes.Path;
                await attachmentService.SaveToEdocDetail(bookbankResultMapEdocDetail);
            }
            
            return Ok();
        }

        [HttpPost("CanselApproval")]
        public async Task<IActionResult> CanselApproval([FromBody] ApprovalViewModel model)
        {         
            return Ok(await approvalService.CanselApprovalAsync(model.AccNo, model.VictimNo, model.AppNo, model.UserIdLine));
        }

        [HttpGet("Invoicedt/{accNo}/{victimNo}/{appNo}")]
        public async Task<IActionResult> GetInvoicedtDetail(string accNo, int victimNo, int appNo)
        {
            return Ok(await approvalService.GetHistoryInvoicedt(accNo.Replace("-", "/"), victimNo, appNo));
        }

        [HttpGet("DataConfirmMoney/{accNo}/{victimNo}/{reqNo}")]
        public async Task<IActionResult> GetDataConfirmMoney(string accNo, int victimNo, int reqNo)
        {
            return Ok(await approvalService.GetDataForConfirmMoney(accNo.Replace("-", "/"), victimNo, reqNo));
        }

        [HttpGet("ApprovalDetail/{accNo}/{victimNo}/{reqNo}/{userIdCard}")]
        public async Task<IActionResult> GetApprovalDataForApprovalDetailPage(string accNo, int victimNo, int reqNo,string userIdCard)
        {
            return Ok(await approvalService.GetApprovalDetail(accNo.Replace("-", "/"), victimNo, reqNo, userIdCard));
        }

        [HttpPost("CheckInvoiceUsing")]
        public async Task<IActionResult> CheckInvoiceUsing([FromBody] List<BillViewModel> models)
        {
            var resultMapToInvoicehd = _mapper.Map<CheckDuplicateInvoiceViewModel[]>(models);                    
            return Ok(await approvalService.CheckDuplicateInvoice(resultMapToInvoicehd));
        }
    }
}
