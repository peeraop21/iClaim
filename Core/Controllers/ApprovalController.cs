using Core.ViewModels;
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
using System.IO;
using System.Net.Http;
using Newtonsoft.Json;
using System.Net;
using Nancy.Json;
using DinkToPdf;
using Core.Models.API;


// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Core.Controllers
{   
    [Route("api/[controller]")]
    [ApiController]
    public class ApprovalController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IApprovalService approvalService;
        private readonly IAccidentService accidentService;
        private readonly IConverter converter;
        private readonly IAttachmentService attachmentService;
        private readonly IMasterService masterService;
        public ApprovalController(IApprovalService approvalService, IAccidentService accidentService, IMapper _mapper, IConverter converter, IAttachmentService attachmentService, IMasterService masterService)
        {
            this.converter = converter;
            this.approvalService = approvalService;
            this._mapper = _mapper;
            this.attachmentService = attachmentService;
            this.masterService = masterService;
            this.accidentService = accidentService;
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
            var result = await approvalService.UpdateApprovalStatusAsync(model.AccNo.Replace('-', '/'), model.VictimNo, model.ReqNo, "ConfirmMoney", false);
            await CreateAndSignBoto(model);
            return Ok(result);
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

        [HttpPost("DownloadPdfBoto3")]
        public async Task<IActionResult> GetPdfBoto3FromECM([FromBody] EdocDetailViewModel model)
        {
            var documentPath = await attachmentService.GetDocumentPath(model);
            var base64pdf = await attachmentService.DownloadFileFromECM(documentPath);
            var pdfBytes = Convert.FromBase64String(base64pdf);
            return File(pdfBytes, "application/pdf");
        }
        private async Task<string> CreateAndSignBoto(ReqDataViewModel model)
        {
            model.Channel = "HOSPITAL";
            var acc = await accidentService.GetAccidentForGenPDF(model.AccNo.Replace("-", "/"), model.VictimNo, model.ReqNo);
            var accVictim = await accidentService.GetAccidentVictim(model.AccNo.Replace("-", "/"), model.Channel, model.UserIdCard, model.VictimNo);
            var accCar = await accidentService.GetAccidentCar(model.AccNo.Replace("-", "/"), model.Channel);
            var approvalData = await approvalService.GetApprovalDataForGenPDF(model.AccNo.Replace("-", "/"), model.VictimNo, model.ReqNo);

            byte[] file = null;
            var globalSettings = new GlobalSettings
            {
                ColorMode = ColorMode.Color,
                Orientation = Orientation.Portrait,
                PaperSize = PaperKind.A4,
                Margins = new MarginSettings { Top = 0, Bottom = 0, Left = 0, Right = 0 },
                DocumentTitle = "บต3",
                DPI = 300,
                ImageDPI = 300,
                Outline = false
            };

            string HtmlContent = string.Empty;
            HtmlContent = await GenBotoBody(acc, accVictim, accCar, approvalData);

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
            var pdfSigned = await SignPdfByByte(file, model.AccNo, model.VictimNo, model.ReqNo);
            if (pdfSigned.Status == 1)
            {
                return pdfSigned.Message;
            }
            else
            {
                return pdfSigned.Message;
            }
            


        }
        private async Task<SignPdfRes> SignPdfByByte(byte[] unSignedPdf, string accNo, int victimNo, int reqNo)
        {
            string URL = "https://ts2digitalsignapi.rvpeservice.com/api/DigitalSignIclaim/SignPdtByte";

            try
            {
                var httpWebRequest = (HttpWebRequest)WebRequest.Create(URL);
                httpWebRequest.ContentType = "application/json";
                httpWebRequest.Method = "POST";

                using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                {
                    string json = new JavaScriptSerializer().Serialize(new
                    {
                        PdfBytes = unSignedPdf,
                        AccNo = accNo,
                        VictimNo = victimNo,
                        ReqNo = reqNo

                    });

                    streamWriter.Write(json);
                    streamWriter.Flush();
                    streamWriter.Close();
                }
                var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    var result = streamReader.ReadToEnd();
                    var resp = JsonConvert.DeserializeObject<SignPdfRes>(result);

                    return resp;
                }
            }
            catch (HttpRequestException e)
            {
                return null;
            }
        }
        private async Task<string> GenBotoBody(AccidentPDFViewModel acc, VictimtViewModel accVictim, CarViewModel accCar, ApprovalPDFViewModel approvalData)
        {
            var template = System.IO.Directory.GetCurrentDirectory() + @"\Templates\Boto3_Template.html";
            using (StreamReader reader = new StreamReader(template))
            {
                var htmlTemplate = await reader.ReadToEndAsync();
                htmlTemplate = htmlTemplate.Replace("{AccNo}", (string.IsNullOrEmpty(acc.AccNo)) ? "-" : acc.AccNo);
                if (approvalData.ClaimNo != null)
                {
                    htmlTemplate = htmlTemplate.Replace("{AccNo}", acc.AccNo + "(" + approvalData.ClaimNo + ")");
                }
                htmlTemplate = htmlTemplate.Replace("{AccVictim.Name}", (string.IsNullOrEmpty(accVictim.Fname)) ? "-" : accVictim.Prefix + accVictim.Fname + " " + accVictim.Lname);
                htmlTemplate = htmlTemplate.Replace("( ) ผู้ประสบภัย", "(&#10004;) ผู้ประสบภัย");
                htmlTemplate = htmlTemplate.Replace("{AccVictim.Age}", (string.IsNullOrEmpty(accVictim.Age.ToString())) ? "-" : accVictim.Age.ToString());
                htmlTemplate = htmlTemplate.Replace("{AccVictim.HomeNo}", (string.IsNullOrEmpty(accVictim.HomeId)) ? "-" : accVictim.HomeId);
                htmlTemplate = htmlTemplate.Replace("{AccVictim.Moo}", (string.IsNullOrEmpty(accVictim.Moo)) ? "-" : accVictim.Moo);
                htmlTemplate = htmlTemplate.Replace("{AccVictim.Soi}", (string.IsNullOrEmpty(accVictim.Soi)) ? "-" : accVictim.Soi);
                htmlTemplate = htmlTemplate.Replace("{AccVictim.Road}", (string.IsNullOrEmpty(accVictim.Road)) ? "-" : accVictim.Road);
                htmlTemplate = htmlTemplate.Replace("{AccVictim.Tumbol}", (string.IsNullOrEmpty(accVictim.TumbolName)) ? "-" : accVictim.TumbolName);
                htmlTemplate = htmlTemplate.Replace("{AccVictim.District}", (string.IsNullOrEmpty(accVictim.DistrictName)) ? "-" : accVictim.DistrictName);
                htmlTemplate = htmlTemplate.Replace("{AccVictim.Province}", (string.IsNullOrEmpty(accVictim.ProvinceName)) ? "-" : accVictim.ProvinceName);
                htmlTemplate = htmlTemplate.Replace("{AccVictim.Zipcode}", (string.IsNullOrEmpty(accVictim.Zipcode)) ? "-" : accVictim.Zipcode);
                htmlTemplate = htmlTemplate.Replace("{AccVictim.TelNo}", (string.IsNullOrEmpty(accVictim.TelNo)) ? "-" : accVictim.TelNo);
                if (string.IsNullOrEmpty(acc.TimeAcc))
                {
                    htmlTemplate = htmlTemplate.Replace("{Acc.DateTime}", acc.DateAccString + " เวลา " + "-" + " น.");
                }
                else
                {
                    htmlTemplate = htmlTemplate.Replace("{Acc.DateTime}", acc.DateAccString + " เวลา " + acc.TimeAcc + " น.");
                }
                htmlTemplate = htmlTemplate.Replace("{Acc.Place}", acc.AccPlace + " จ." + acc.AccProv);
                htmlTemplate = htmlTemplate.Replace("{AccCar.FoundCarLicense}", (string.IsNullOrEmpty(accCar.FoundCarLicense)) ? "-" : accCar.FoundCarLicense);
                htmlTemplate = htmlTemplate.Replace("{AccCar.FoundChassisNo}", (string.IsNullOrEmpty(accCar.FoundChassisNo)) ? "-" : accCar.FoundChassisNo);
                htmlTemplate = htmlTemplate.Replace("{AccCar.FoundPolicyNo}", (string.IsNullOrEmpty(accCar.FoundPolicyNo)) ? "-" : accCar.FoundPolicyNo);
                htmlTemplate = htmlTemplate.Replace("( ) รถคันเดียว ไม่มีคู่กรณี", "(&#10004;) รถคันเดียว ไม่มีคู่กรณี");
                if (accVictim.VictimIs == "ผขป")
                {
                    htmlTemplate = htmlTemplate.Replace("( ) ผู้ขับขี่", "(&#10004;) ผู้ขับขี่");
                }
                else if (accVictim.VictimIs == "ผสป")
                {
                    htmlTemplate = htmlTemplate.Replace("( ) ผู้โดยสารรถคันเอาประกันภัย", "(&#10004;) ผู้โดยสารรถคันเอาประกันภัย");
                }
                if (accVictim.VictimType == "IPD")
                {
                    htmlTemplate = htmlTemplate.Replace("( ) ผู้ป่วยใน", "(&#10004;) ผู้ป่วยใน");
                }
                else
                {
                    htmlTemplate = htmlTemplate.Replace("( ) ผู้ป่วยนอก", "(&#10004;) ผู้ป่วยนอก");
                }
                htmlTemplate = htmlTemplate.Replace("{AccVictim.DetailBroken}", (string.IsNullOrEmpty(accVictim.DetailBroken)) ? "-" : accVictim.DetailBroken);
                htmlTemplate = htmlTemplate.Replace("{AccVictim.TakenDate}", "-");
                htmlTemplate = htmlTemplate.Replace("( ) ค่ารักษาพยาบาลและค่าใช้จ่ายอันจำเป็นเกี่ยวกับการรักษาพยาบาล", (string.IsNullOrEmpty(approvalData.CureMoney.ToString())) ? "( ) ค่ารักษาพยาบาลและค่าใช้จ่ายอันจำเป็นเกี่ยวกับการรักษาพยาบาล" : "(&#10004;) ค่ารักษาพยาบาลและค่าใช้จ่ายอันจำเป็นเกี่ยวกับการรักษาพยาบาล");
                htmlTemplate = htmlTemplate.Replace("{ApprovalData.CureMoney}", approvalData.CureMoney.ToString());

                if (approvalData.IsEverAuthorize == true)
                {
                    htmlTemplate = htmlTemplate.Replace("( ) เคย", "(&#10004;) เคย");
                    htmlTemplate = htmlTemplate.Replace("{ApprovalData.EverAuthorizeMoney}", (string.IsNullOrEmpty(approvalData.EverAuthorizeMoney.ToString())) ? "-" : approvalData.EverAuthorizeMoney.ToString());
                    htmlTemplate = htmlTemplate.Replace("{ApprovalData.EverAuthorizeHosId}", (string.IsNullOrEmpty(approvalData.EverAuthorizeHosId.ToString())) ? "-" : approvalData.EverAuthorizeHosId.ToString());
                }
                else
                {
                    htmlTemplate = htmlTemplate.Replace("( ) ไม่เคย", "(&#10004;) ไม่เคย");
                    htmlTemplate = htmlTemplate.Replace("{ApprovalData.EverAuthorizeMoney}", "");
                    htmlTemplate = htmlTemplate.Replace("{ApprovalData.EverAuthorizeHosId}", "");
                }

                htmlTemplate = htmlTemplate.Replace("{ApprovalData.OtpSign}", "ยื่นผ่าน iClaim ด้วย SMS OTP (ref: " + approvalData.OtpSign + ")");

                htmlTemplate = htmlTemplate.Replace("( ) ใบเสร็จรับเงิน", (string.IsNullOrEmpty(approvalData.CureMoney.ToString())) ? "( ) ใบเสร็จรับเงิน" : "(&#10004;) ใบเสร็จรับเงิน");
                htmlTemplate = htmlTemplate.Replace("{ApprovalData.IdInvhd}", approvalData.IdInvhd.ToString());
                htmlTemplate = htmlTemplate.Replace("{ApprovalData.InvCount}", approvalData.InvCount.ToString());
                htmlTemplate = htmlTemplate.Replace("{ApprovalData.HosId}", approvalData.HosId.ToString());
                htmlTemplate = htmlTemplate.Replace("{ApprovalData.RecordDay}", approvalData.RecordDay);
                htmlTemplate = htmlTemplate.Replace("{ApprovalData.RecordMonth}", approvalData.RecordMonth);
                htmlTemplate = htmlTemplate.Replace("{ApprovalData.RecordYear}", approvalData.RecordYear);
                htmlTemplate = htmlTemplate.Replace("{ApprovalData.TextCureMoney}", approvalData.TextCureMoney);

                htmlTemplate = htmlTemplate.Replace("( ) บัตรประจำตัวผู้ประสบภัย", "(&#10004;) บัตรประจำตัวผู้ประสบภัย");
                return htmlTemplate;
            }
        }
    }
}
