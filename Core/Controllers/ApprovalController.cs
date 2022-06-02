using System.Threading.Tasks;
using AutoMapper;
using DataAccess.EFCore.DigitalClaimModels;
using DinkToPdf.Contracts;
using Microsoft.AspNetCore.Mvc;
using Services;
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
using Core.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Cors;
using Services.Models;
using Microsoft.Extensions.Logging;


// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Core.Controllers
{
    [EnableCors("iClaim")]
    [Route("api/[controller]")]
    [ApiController]
    public class ApprovalController : ControllerBase
    {
        private readonly IConfiguration configuration;
        private readonly IMapper _mapper;
        private readonly IApprovalService approvalService;
        private readonly IAccidentService accidentService;
        private readonly IConverter converter;
        private readonly IAttachmentService attachmentService;
        private readonly IMasterService masterService;
        private readonly ILogger<ApprovalController> logger;
        public ApprovalController(IConfiguration configuration, IApprovalService approvalService, IAccidentService accidentService, IMapper _mapper, IConverter converter, IAttachmentService attachmentService, IMasterService masterService, ILogger<ApprovalController> logger)
        {
            this.configuration = configuration;
            this.converter = converter;
            this.approvalService = approvalService;
            this._mapper = _mapper;
            this.attachmentService = attachmentService;
            this.masterService = masterService;
            this.accidentService = accidentService;
            this.logger = logger;
        }

        [Authorize]
        [HttpPost("HistoryRights")]
        public async Task<IActionResult> GetHistoryRights([FromBody] ApprovalReq appReq)
        {
            try
            {
                return Ok(await approvalService.GetApprovalRegis(appReq.AccNo.Replace("-", "/"), appReq.VictimNo, appReq.RightsType));
            }
            catch (Exception ex)
            {
                string baseUrl = configuration["BaseUrl:Publish"];
                if (!string.IsNullOrEmpty(appReq.UserIdLine))
                {
                    logger.LogError(baseUrl + ", API: GetHistoryRight, User: {0}, Exception: {1}", appReq.UserIdLine, ex);
                    return StatusCode(500);
                }
                else
                {
                    logger.LogError(baseUrl + ", API: GetHistoryRight, Exception: {0}", ex);
                    return StatusCode(500);
                }
            }
            
        }

        [Authorize]
        [HttpPost("IclaimApproval")]
        public async Task<IActionResult> GetIclaimApproval([FromBody] ReqData req)
        {
            try
            {
                return Ok(await approvalService.GetIClaimApprovalAsync(req.AccNo.Replace("-", "/"), req.VictimNo));
            }
            catch (Exception ex)
            {
                string baseUrl = configuration["BaseUrl:Publish"];
                if (!string.IsNullOrEmpty(req.UserIdLine))
                {
                    logger.LogError(baseUrl + ", API: GetIclaimApproval, User: {0}, Exception: {1}", req.UserIdLine, ex);
                    return StatusCode(500);
                }
                else
                {
                    logger.LogError(baseUrl + ", API: GetIclaimApproval, User: {0}, Exception: {1}", req.UserIdCard, ex);
                    return StatusCode(500);
                }
            }
            
        }

        
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> PostAsync([FromBody] ApprovalReq model)
        {
            try
            {
                model.BankData.accountNumber = model.BankData.accountNumber.Replace("-", "");
                var resultMapIclaimApproval = _mapper.Map<DataAccess.EFCore.DigitalClaimModels.IclaimApproval>(model);
                var resultMapBank = _mapper.Map<InputBank>(model.BankData);
                var resultMapVictim = _mapper.Map<Victim>(model.VictimData);
                var resultMapToInvoicehd = _mapper.Map<Invoicehd[]>(model.BillsData);

                var result = await approvalService.AddAsync(resultMapIclaimApproval, resultMapBank, resultMapVictim, resultMapToInvoicehd, model.UserIdLine);
                ECM ecmModel = new ECM();

                for (int i = 0; i < model.BillsData.Count; i++)
                {
                    for (int j = 0; j < model.BillsData[i].billFileShow.Count; j++)
                    {
                        ecmModel.SystemId = "02";
                        ecmModel.TemplateId = "03";
                        ecmModel.DocID = "01";

                        ecmModel.RefNo = result[i].IdInvhd + "|" + result[0].AccNo + "|" + result[0].VictimNo + "-" + (j + 1);
                        ecmModel.FileName = DateTime.Now.ToString("ddMMyyyyHHmmss") + model.BillsData[i].filename[j];
                        ecmModel.Base64String = model.BillsData[i].billFileShow[j];
                        var invoiceEcmRes = await attachmentService.UploadFileToECM(ecmModel);
                        var resultMapEdocDetail = _mapper.Map<DocumentDetail>(ecmModel);
                        resultMapEdocDetail.Paths = invoiceEcmRes.Path;
                        await attachmentService.SaveToEdocDetail(resultMapEdocDetail);
                    }

                }
                ecmModel.SystemId = "03";
                ecmModel.TemplateId = "09";
                ecmModel.DocID = "01";

                ecmModel.RefNo = result[0].IclaimAppNo + "|" + result[0].AccNo + "|" + result[0].VictimNo;
                ecmModel.FileName = (string.IsNullOrEmpty(model.BankData.bankFilename)) ? DateTime.Now.ToString("ddMMyyyyHHmmss") + "bankold.png" : DateTime.Now.ToString("ddMMyyyyHHmmss") + model.BankData.bankFilename;
                ecmModel.Base64String = model.BankData.bankBase64String;
                var bookbankEcmRes = await attachmentService.UploadFileToECM(ecmModel);
                var bookbankResultMapEdocDetail = _mapper.Map<DocumentDetail>(ecmModel);
                bookbankResultMapEdocDetail.Paths = bookbankEcmRes.Path;
                await attachmentService.SaveToEdocDetail(bookbankResultMapEdocDetail);

                return Ok(new { reqNo = result[0].IclaimAppNo });
            }
            catch (Exception ex)
            {
                string baseUrl = configuration["BaseUrl:Publish"];
                if (!string.IsNullOrEmpty(model.UserIdLine))
                {
                    logger.LogError(baseUrl + ", API: PostApproval, User: {0}, Exception: {1}", model.UserIdLine, ex);
                    return StatusCode(500);
                }
                else
                {
                    logger.LogError(baseUrl + ", API: PostApproval, Exception: {0}", ex);
                    return StatusCode(500);
                }
            }

           
        }
     
        [Authorize]
        [HttpPost("ConfirmMoney")]
        public async Task<IActionResult> ConfirmMoney([FromBody] ReqData req)
        {
            try
            {
                var result = await approvalService.UpdateApprovalStatusAsync(req.AccNo.Replace('-', '/'), req.VictimNo, req.ReqNo, "ConfirmMoney", false);
                await CreateAndSignBoto(req);
                return Ok(result);
            }
            catch (Exception ex)
            {
                string baseUrl = configuration["BaseUrl:Publish"];
                if (!string.IsNullOrEmpty(req.UserIdLine))
                {
                    logger.LogError(baseUrl + ", API: ConfirmMoney, User: {0}, Exception: {1}", req.UserIdLine, ex);
                    return StatusCode(500);
                }
                else
                {
                    logger.LogError(baseUrl + ", API: ConfirmMoney, User: {0}, Exception: {1}", req.UserIdCard, ex);
                    return StatusCode(500);
                }
            }
            
        }

        [Authorize]
        [HttpPost("LastDocumentReceive")]
        public async Task<IActionResult> GetLastDocumentReceive([FromBody] ReqData req)
        {
            try
            {
                return Ok(await approvalService.GetLastIClaimBankAccountAsync(req.AccNo.Replace("-", "/"), req.VictimNo));
            }
            catch (Exception ex)
            {
                string baseUrl = configuration["BaseUrl:Publish"];
                if (!string.IsNullOrEmpty(req.UserIdLine))
                {
                    logger.LogError(baseUrl + ", API: GetLastDocumentReceive, User: {0}, Exception: {1}", req.UserIdLine, ex);
                    return StatusCode(500);
                }
                else
                {
                    logger.LogError(baseUrl + ", API: GetLastDocumentReceive, User: {0}, Exception: {1}", req.UserIdCard, ex);
                    return StatusCode(500);
                }
            }
            
        }

       
        [Authorize]//-dev
        [HttpPost("DataForEditApprovalPage")]
        public async Task<IActionResult> GetDataForEditApprovalPage([FromBody] ReqData req)
        {
            try
            {
                var wounded = await masterService.GetWoundeds();
                var typesOfInvoiceNotPass = await masterService.GetTypesOfInvoiceNotPass();
                var invoicesNotPass = await approvalService.GetInvoicehdAsync(req.AccNo.Replace("-", "/"), req.VictimNo, req.ReqNo, 2);
                var changwats = await masterService.GetChangwatsAsync();
                var banksName = await masterService.GetBank();
                var typesOfBankAccountNotPass = await masterService.GetTypesOfBankAccountNotPass();
                var account = await approvalService.GetIClaimBankAccountAsync(req.AccNo.Replace("-", "/"), req.VictimNo, req.ReqNo);
                var accountChecks = await approvalService.GetDocumentCheck(req.AccNo.Replace("-", "/"), req.VictimNo, req.ReqNo);
                return Ok(new
                {
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
            catch (Exception ex)
            {
                string baseUrl = configuration["BaseUrl:Publish"];
                if (!string.IsNullOrEmpty(req.UserIdLine))
                {
                    logger.LogError(baseUrl + ", API: GetDataForEditApprovalPage, User: {0}, Exception: {1}", req.UserIdLine, ex);
                    return StatusCode(500);
                }
                else
                {
                    logger.LogError(baseUrl + ", API: GetDataForEditApprovalPage, User: {0}, Exception: {1}", req.UserIdCard, ex);
                    return StatusCode(500);
                }
            }
            
        }


        [Authorize]
        [HttpPost("DownloadFromECM")]
        public async Task<IActionResult> GetBase64FromECM([FromBody] DocumentDetail req)
        {
            try
            {
                var documentPath = await attachmentService.GetDocumentPath(req);
                List<string> base64List = new List<string>();
                for (int i = 0; i < documentPath.Count; i++)
                {
                    base64List.Add(await attachmentService.DownloadFileFromECM(documentPath[i].Paths));
                }
                return Ok(base64List);
            }
            catch (Exception ex)
            {
                string baseUrl = configuration["BaseUrl:Publish"];
                logger.LogError(baseUrl + ", API: GetBase64FromECM, Exception: {0}", ex);
                return StatusCode(500);
            }
            
        }

        [Authorize]
        [HttpPost("UpdateApproval")]
        public async Task<IActionResult> UpdateApproval([FromBody] ApprovalReq model)
        {
            try
            {
                model.BankData.accountNumber = model.BankData.accountNumber.Replace("-", "");
                var resultMapBank = _mapper.Map<UpdateBank>(model.BankData);
                var resultMapToInvoicehd = _mapper.Map<UpdateInvoice[]>(model.BillsData);
                var result = await approvalService.UpdateAsync(model.AccNo, model.VictimNo, model.AppNo, model.UserIdLine, resultMapBank, resultMapToInvoicehd);
                ECM ecmModel = new ECM();

                if (model.BillsData != null)
                {
                    for (int i = 0; i < model.BillsData.Count; i++)
                    {
                        if (resultMapToInvoicehd[i].isEditImage && !resultMapToInvoicehd[i].isCancel)
                        {
                            for (int j = 0; j < resultMapToInvoicehd[i].editBillImage.Count; j++)
                            {
                                ecmModel.SystemId = "02";
                                ecmModel.TemplateId = "03";
                                ecmModel.DocID = "01";
                                ecmModel.RefNo = resultMapToInvoicehd[i].billNo + "|" + model.AccNo + "|" + model.VictimNo + "-" + (j + 1)  /*+ "|" + model.AppNo*/;
                                ecmModel.FileName = DateTime.Now.ToString("ddMMyyyyHHmmss") + resultMapToInvoicehd[i].filename[j];
                                ecmModel.Base64String = resultMapToInvoicehd[i].editBillImage[j];
                                var invoiceEcmRes = await attachmentService.UploadFileToECM(ecmModel);
                                var resultMapEdocDetail = _mapper.Map<DocumentDetail>(ecmModel);
                                resultMapEdocDetail.Paths = invoiceEcmRes.Path;
                                await attachmentService.SaveToEdocDetail(resultMapEdocDetail);
                            }
                        }
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
                    var bookbankResultMapEdocDetail = _mapper.Map<DocumentDetail>(ecmModel);
                    bookbankResultMapEdocDetail.Paths = bookbankEcmRes.Path;
                    await attachmentService.SaveToEdocDetail(bookbankResultMapEdocDetail);
                }
                return Ok();
            }
            catch (Exception ex)
            {
                string baseUrl = configuration["BaseUrl:Publish"];
                if (!string.IsNullOrEmpty(model.UserIdLine))
                {

                    logger.LogError(baseUrl + ", API: UpdateApproval, User: {0}, Exception: {1}", model.UserIdLine, ex);
                    return StatusCode(500);
                }
                else
                {
                    logger.LogError(baseUrl + ", API: UpdateApproval, Exception: {0}", ex);
                    return StatusCode(500);
                }

            }
            
        }

        [Authorize]
        [HttpPost("CanselApproval")]
        public async Task<IActionResult> CanselApproval([FromBody] ApprovalReq req)
        {
            try
            {
                return Ok(await approvalService.CanselApprovalAsync(req.AccNo, req.VictimNo, req.AppNo, req.UserIdLine));
            }catch (Exception ex)
            {
                string baseUrl = configuration["BaseUrl:Publish"];
                if (!string.IsNullOrEmpty(req.UserIdLine))
                {
                    logger.LogError(baseUrl + ", API: CanselApproval, User: {0}, Exception: {1}", req.UserIdLine, ex);
                    return StatusCode(500);
                }
                else
                {
                    logger.LogError(baseUrl + ", API: CanselApproval, Exception: {0}", ex);
                    return StatusCode(500);
                }
            }
            
        }

        [Authorize]
        [HttpPost("Invoicedt")]
        public async Task<IActionResult> GetInvoicedtDetail([FromBody] ApprovalReq req)
        {
            try
            {
                return Ok(await approvalService.GetHistoryInvoicedt(req.AccNo.Replace("-", "/"), req.VictimNo, req.AppNo));
            }
            catch (Exception ex)
            {
                string baseUrl = configuration["BaseUrl:Publish"];
                if (!string.IsNullOrEmpty(req.UserIdLine))
                {
                    logger.LogError(baseUrl + ", API: GetInvoicedtDetail, User: {0}, Exception: {1}", req.UserIdLine, ex);
                    return StatusCode(500);
                }
                else
                {
                    logger.LogError(baseUrl + ", API: GetInvoicedtDetail, Exception: {0}", ex);
                    return StatusCode(500);
                }
            }
            
        }

        [Authorize]
        [HttpPost("DataConfirmMoney")]
        public async Task<IActionResult> GetDataConfirmMoney([FromBody] ReqData req)
        {
            try
            {
                var initConfirmMoney = await approvalService.GetDataForConfirmMoney(req.AccNo.Replace("-", "/"), req.VictimNo, req.ReqNo);
                var banks = await masterService.GetBank();
                return Ok(new { ConfirmMoneyData = initConfirmMoney, Banks = banks });
            }catch (Exception ex)
            {
                string baseUrl = configuration["BaseUrl:Publish"];
                if (!string.IsNullOrEmpty(req.UserIdLine))
                {
                    logger.LogError(baseUrl + ", API: GetDataConfirmMoney, User: {0}, Exception: {1}", req.UserIdLine, ex);
                    return StatusCode(500);
                }
                else
                {
                    logger.LogError(baseUrl + ", API: GetDataConfirmMoney, User: {0}, Exception: {1}", req.UserIdCard, ex);
                    return StatusCode(500);
                }
            }
            
        }

        [Authorize]
        [HttpPost("ApprovalDetail")]
        public async Task<IActionResult> GetApprovalDataForApprovalDetailPage([FromBody] ReqData req)
        {
            try
            {
                return Ok(await approvalService.GetApprovalDetail(req.AccNo.Replace("-", "/"), req.VictimNo, req.ReqNo, req.UserIdCard));
            }
            catch (Exception ex)
            {
                string baseUrl = configuration["BaseUrl:Publish"];
                if (!string.IsNullOrEmpty(req.UserIdLine))
                {
                    logger.LogError(baseUrl + ", API: GetApprovalDataForApprovalDetailPage, User: {0}, Exception: {1}", req.UserIdLine, ex);
                    return StatusCode(500);
                }
                else
                {
                    logger.LogError(baseUrl + ", API: GetApprovalDataForApprovalDetailPage, User: {0}, Exception: {1}", req.UserIdCard, ex);
                    return StatusCode(500);
                }
            }
            
        }

        [Authorize]
        [HttpPost("CheckInvoiceUsing")]
        public async Task<IActionResult> CheckInvoiceUsing([FromBody] List<Bill> models)
        {
            try
            {
                var resultMapToInvoicehd = _mapper.Map<CheckDuplicateInvoice[]>(models);
                return Ok(await approvalService.CheckDuplicateInvoice(resultMapToInvoicehd));
            }
            catch (Exception ex)
            {
                string baseUrl = configuration["BaseUrl:Publish"];
                logger.LogError(baseUrl + ", API: CheckInvoiceUsing, Exception: {0}", ex);
                return StatusCode(500);
            }
            
        }

        [Authorize]
        [HttpPost("DownloadPdfBoto3")]
        public async Task<IActionResult> GetPdfBoto3FromECM([FromBody] DocumentDetail model)
        {
            try
            {
                var documentPath = await attachmentService.GetDocumentPath(model);
                var base64pdf = await attachmentService.DownloadFileFromECM(documentPath[0].Paths);
                var pdfBytes = Convert.FromBase64String(base64pdf);
                return File(pdfBytes, "application/pdf");
            }catch (Exception ex)
            {
                string baseUrl = configuration["BaseUrl:Publish"];
                logger.LogError(baseUrl + ", API: GetPdfBoto3FromECM, Exception: {0}", ex);
                return StatusCode(500);
            }
            
        }

        [Authorize]
        [HttpPost("CreateAndSignBoto3")]
        public async Task<string> CreateAndSignBoto(ReqData model)
        {
            try
            {
                var acc = await accidentService.GetAccidentDetail(model.AccNo.Replace("-", "/"));
                var accVictim = await accidentService.GetAccidentVictim(model.AccNo.Replace("-", "/"), model.UserIdCard, model.VictimNo);
                var accCar = await accidentService.GetAccidentCar(model.AccNo.Replace("-", "/"));
                var approvalData = await approvalService.GetApprovalDataForGenPDF(model.AccNo.Replace("-", "/"), model.VictimNo, model.ReqNo);
                if (acc == null || accVictim == null || accCar == null || approvalData == null)
                {
                    return "Something went wrong!";
                }
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
                var pdfSigned = await SignPdfByByte(file, model.AccNo.Replace("/", "-"), model.VictimNo, model.ReqNo);
                if (pdfSigned.Status == 1)
                {
                    return pdfSigned.Message;
                }
                else
                {
                    return pdfSigned.Message;
                }
            }catch (Exception ex)
            {
                string baseUrl = configuration["BaseUrl:Publish"];
                if (!string.IsNullOrEmpty(model.UserIdLine))
                {
                    logger.LogError(baseUrl + ", API: CreateAndSignBoto, User: {0}, Exception: {1}", model.UserIdLine, ex);
                    return ex.Message;
                }
                else
                {
                    logger.LogError(baseUrl + ", API: CreateAndSignBoto, User: {0}, Exception: {1}", model.UserIdCard, ex);
                    return ex.Message;
                }
            }                 
        }
        private async Task<SignPdfRes> SignPdfByByte(byte[] unSignedPdf, string accNo, int victimNo, int reqNo)
        {
            string URL = configuration["API:Attachment:DigitalSignPdf"];

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
            catch (HttpRequestException ex)
            {
                string baseUrl = configuration["BaseUrl:Publish"];
                logger.LogError(baseUrl + ", API: SignPdfByByte, Exception: {0}", ex);
                return null;
            }
        }
        private async Task<string> GenBotoBody(AccidentDetail acc, Victim accVictim, CarHasPolicy accCar, ApprovalPDF approvalData)
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
