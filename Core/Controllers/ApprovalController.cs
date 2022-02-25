﻿using Core.ViewModels;
using System.Threading.Tasks;
using AutoMapper;
using DataAccess.EFCore.DigitalClaimModels;
using DinkToPdf.Contracts;
using Microsoft.AspNetCore.Mvc;
using Services;
using Services.ViewModels;
using System;
using System.Collections.Generic;
using DataAccess.EFCore.RvpOfficeModels;
using Microsoft.AspNetCore.Authorization;


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
        private readonly IMasterService masterService;
        public ApprovalController(IApprovalService approvalService, IMapper _mapper, IConverter converter, IAttachmentService attachmentService, IMasterService masterService)
        {
            this.converter = converter;
            this.approvalService = approvalService;
            this._mapper = _mapper;
            this.attachmentService = attachmentService;
            this.masterService = masterService;
        }

        [Authorize]
        [HttpGet("{accNo}/{victimNo}/{rightsType}")]
        public async Task<IActionResult> GetApproval(string accNo, int victimNo, int rightsType)
        {
            return Ok(await approvalService.GetApprovalRegis(accNo.Replace("-", "/"), victimNo, rightsType));
        }

        [Authorize]
        [HttpGet("IclaimApproval/{accNo}/{victimNo}")]
        public async Task<IActionResult> GetIclaimApproval(string accNo, int victimNo)
        {
            return Ok(await approvalService.GetIClaimApprovalAsync(accNo.Replace("-", "/"), victimNo));
        }

        [Authorize]
        [HttpGet("DocumentReceive/{accNo}/{victimNo}/{appNo}")]
        public async Task<IActionResult> GetDocumentReceive(string accNo, int victimNo, int appNo)
        {
            return Ok(await approvalService.GetIClaimBankAccountAsync(accNo.Replace("-", "/"), victimNo, appNo));
        }
        //[Authorize]//-dev
        //[HttpPost("AccountAndMasterData")]
        //public async Task<IActionResult> GetAccountAndMasterData([FromBody] ReqDataViewModel model)
        //{
        //    var banksName = await masterService.GetBank();
        //    var account = await approvalService.GetIClaimBankAccountAsync(model.AccNo.Replace("-","/"), model.VictimNo, model.ReqNo);
        //    return Ok(new { Account = account, BankNames = banksName });
        //}

        // POST api/<ApprovalController>
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> PostAsync([FromBody] ApprovalViewModel model)
        {
            model.BankData.accountNumber = model.BankData.accountNumber.Replace("-", "");
            var resultMapIclaimApproval = _mapper.Map<IclaimApproval>(model);
            var resultMapBank = _mapper.Map<InputBankViewModel>(model.BankData);
            var resultMapVictim = _mapper.Map<VictimtViewModel>(model.VictimData);
            var resultMapToInvoicehd = _mapper.Map<Invoicehd[]>(model.BillsData);

            var result = await approvalService.AddAsync(resultMapIclaimApproval, resultMapBank, resultMapVictim, resultMapToInvoicehd, model.UserIdLine);
            ECMViewModel ecmModel = new ECMViewModel();
            
            for (int i = 0; i < model.BillsData.Count; i++)
            {             
                ecmModel.SystemId = "02";
                ecmModel.TemplateId = "03";
                ecmModel.DocID = "01";

                ecmModel.RefNo = result[i].IdInvhd + "|" + result[0].AccNo + "|" + result[0].VictimNo /* + "|" + result[0].AppNo*/;
                ecmModel.FileName = DateTime.Now.ToString("ddMMyyyyHHmmss") + model.BillsData[i].filename;
                ecmModel.Base64String = model.BillsData[i].billFileShow;
                var invoiceEcmRes = await attachmentService.UploadFileToECM(ecmModel);
                var resultMapEdocDetail = _mapper.Map<EdocDetailViewModel>(ecmModel);
                resultMapEdocDetail.Paths = invoiceEcmRes.Path;
                await attachmentService.SaveToEdocDetail(resultMapEdocDetail);
            }    
            ecmModel.SystemId = "03";
            ecmModel.TemplateId = "09";
            ecmModel.DocID = "01";
           
            ecmModel.RefNo = result[0].IclaimAppNo + "|" + result[0].AccNo + "|" + result[0].VictimNo;
            ecmModel.FileName = (string.IsNullOrEmpty(model.BankData.bankFilename)) ? DateTime.Now.ToString("ddMMyyyyHHmmss") + "bankold.png" : DateTime.Now.ToString("ddMMyyyyHHmmss") + model.BankData.bankFilename;
            ecmModel.Base64String = model.BankData.bankBase64String;
            var bookbankEcmRes = await attachmentService.UploadFileToECM(ecmModel);
            var bookbankResultMapEdocDetail = _mapper.Map<EdocDetailViewModel>(ecmModel);
            bookbankResultMapEdocDetail.Paths = bookbankEcmRes.Path;
            await attachmentService.SaveToEdocDetail(bookbankResultMapEdocDetail);

            return Ok(new { reqNo = result[0].IclaimAppNo });
        }
     
        [Authorize]
        [HttpPost("ConfirmMoney")]
        public async Task<IActionResult> ConfirmMoney([FromBody] ReqDataViewModel model)
        {
            return Ok(await approvalService.UpdateApprovalStatusAsync(model.AccNo.Replace('-', '/'), model.VictimNo, model.ReqNo, "ConfirmMoney", false));
        }

        [Authorize]
        [HttpGet("LastDocumentReceive/{accNo}/{victimNo}")]
        public async Task<IActionResult> GetLastDocumentReceive(string accNo, int victimNo)
        {
            return Ok(await approvalService.GetLastIClaimBankAccountAsync(accNo.Replace("-", "/"), victimNo));
        }

        //[Authorize]
        //[HttpGet("InvoicehdNotPass/{accNo}/{victimNo}/{appNo}")]
        //public async Task<IActionResult> GetInvoicehdNotPass(string accNo, int victimNo, int appNo)
        //{
        //    var typesOfInvoiceNotPass = await masterService.GetTypesOfInvoiceNotPass();
        //    var invoicesNotPass = await approvalService.GetInvoicehdAsync(accNo.Replace("-", "/"), victimNo, appNo, 2);
        //    return Ok( new { TypesOfInvoiceNotPass = typesOfInvoiceNotPass, InvoicesNotPass = invoicesNotPass });
        //}
        [Authorize]//-dev
        [HttpGet("DataForEditApprovalPage/{accNo}/{victimNo}/{reqNo}")]
        public async Task<IActionResult> GetDataForEditApprovalPage(string accNo, int victimNo, int reqNo)
        {
            var wounded = await masterService.GetWoundeds();
            var typesOfInvoiceNotPass = await masterService.GetTypesOfInvoiceNotPass();
            var invoicesNotPass = await approvalService.GetInvoicehdAsync(accNo.Replace("-", "/"), victimNo, reqNo, 2);
            var changwats = await masterService.GetChangwat();
            var banksName = await masterService.GetBank();
            var typesOfBankAccountNotPass = await masterService.GetTypesOfBankAccountNotPass();
            var account = await approvalService.GetIClaimBankAccountAsync(accNo.Replace("-", "/"), victimNo, reqNo);
            var accountChecks = await approvalService.GetDocumentCheck(accNo.Replace("-", "/"), victimNo, reqNo);
            return Ok(new { 
                Woundeds = wounded,
                TypesOfInvoiceNotPass = typesOfInvoiceNotPass, 
                InvoicesNotPass = invoicesNotPass,
                Changwats = changwats,
                BankNames = banksName,
                TypesOfBankAccountNotPass = typesOfBankAccountNotPass,
                Account = account,
                accountChecks = accountChecks

                
            });
        }

        [Authorize]
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
        [Authorize]
        [HttpPost("DownloadFromECM")]
        public async Task<IActionResult> GetBase64FromECM([FromBody] EdocDetailViewModel model)
        {
            var documentPath = await attachmentService.GetDocumentPath(model);            
            return Ok(await attachmentService.DownloadFileFromECM(documentPath));
        }

        [Authorize]
        [HttpPost("UpdateApproval")]
        public async Task<IActionResult> UpdateApproval([FromBody] ApprovalViewModel model)
        {
            model.BankData.accountNumber = model.BankData.accountNumber.Replace("-", "");
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

        [Authorize]
        [HttpPost("CanselApproval")]
        public async Task<IActionResult> CanselApproval([FromBody] ApprovalViewModel model)
        {         
            return Ok(await approvalService.CanselApprovalAsync(model.AccNo, model.VictimNo, model.AppNo, model.UserIdLine));
        }

        [Authorize]
        [HttpGet("Invoicedt/{accNo}/{victimNo}/{appNo}")]
        public async Task<IActionResult> GetInvoicedtDetail(string accNo, int victimNo, int appNo)
        {
            return Ok(await approvalService.GetHistoryInvoicedt(accNo.Replace("-", "/"), victimNo, appNo));
        }

        [Authorize]
        [HttpPost("DataConfirmMoney")]
        public async Task<IActionResult> GetDataConfirmMoney([FromBody] ReqDataViewModel model)
        {
            var initConfirmMoney = await approvalService.GetDataForConfirmMoney(model.AccNo.Replace("-", "/"), model.VictimNo, model.ReqNo);
            var banks = await masterService.GetBank();
            return Ok(new {ConfirmMoneyData = initConfirmMoney, Banks = banks });
        }

        [Authorize]
        [HttpGet("ApprovalDetail/{accNo}/{victimNo}/{reqNo}/{userIdCard}")]
        public async Task<IActionResult> GetApprovalDataForApprovalDetailPage(string accNo, int victimNo, int reqNo,string userIdCard)
        {
            return Ok(await approvalService.GetApprovalDetail(accNo.Replace("-", "/"), victimNo, reqNo, userIdCard));
        }

        [Authorize]
        [HttpPost("CheckInvoiceUsing")]
        public async Task<IActionResult> CheckInvoiceUsing([FromBody] List<BillViewModel> models)
        {
            var resultMapToInvoicehd = _mapper.Map<CheckDuplicateInvoiceViewModel[]>(models);                    
            return Ok(await approvalService.CheckDuplicateInvoice(resultMapToInvoicehd));
        }
    }
}
