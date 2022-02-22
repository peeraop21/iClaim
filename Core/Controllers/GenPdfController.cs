using System.IO;
using System.Threading.Tasks;
using Core.ViewModels;
using DinkToPdf;
using DinkToPdf.Contracts;
using Microsoft.AspNetCore.Mvc;
using Services;
using Services.ViewModels;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Core.Controllers
{


    [Route("api/[controller]")]
    [ApiController]
    public class GenPdfController : ControllerBase
    {
        
        

        private readonly IConverter converter;
        private readonly IAccidentService accidentService;
        private readonly IApprovalService approvalService;
        public GenPdfController( IConverter converter, IAccidentService accidentService, IApprovalService approvalService)
        {
            this.converter = converter;
            this.accidentService = accidentService;
            this.approvalService = approvalService;
        }
        

        //[HttpGet("GetBoto3/{accNo}/{victimNo}/{appNo}/{channel}")]
        [HttpPost]
        public async Task<IActionResult> GetPDF([FromBody]PostDataViewModel model)
        {
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
            return File(file, "application/pdf");
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
                htmlTemplate = htmlTemplate.Replace("{AccVictim.Soi}", (string.IsNullOrEmpty(accVictim.Soi)) ? "-": accVictim.Soi);
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
                }else if (accVictim.VictimIs == "ผสป")
                {
                    htmlTemplate = htmlTemplate.Replace("( ) ผู้โดยสารรถคันเอาประกันภัย", "(&#10004;) ผู้โดยสารรถคันเอาประกันภัย");
                }
                if (accVictim.VictimType == "IPD")
                {
                    htmlTemplate = htmlTemplate.Replace("( ) ผู้ป่วยใน", "(&#10004;) ผู้ป่วยใน");
                }else
                {
                    htmlTemplate = htmlTemplate.Replace("( ) ผู้ป่วยนอก", "(&#10004;) ผู้ป่วยนอก");
                }
                htmlTemplate = htmlTemplate.Replace("{AccVictim.DetailBroken}", (string.IsNullOrEmpty(accVictim.DetailBroken)) ? "-" : accVictim.DetailBroken);
                htmlTemplate = htmlTemplate.Replace("{AccVictim.TakenDate}",  "-");
                htmlTemplate = htmlTemplate.Replace("( ) ค่ารักษาพยาบาลและค่าใช้จ่ายอันจำเป็นเกี่ยวกับการรักษาพยาบาล", (string.IsNullOrEmpty(approvalData.CureMoney.ToString())) ? "( ) ค่ารักษาพยาบาลและค่าใช้จ่ายอันจำเป็นเกี่ยวกับการรักษาพยาบาล" : "(&#10004;) ค่ารักษาพยาบาลและค่าใช้จ่ายอันจำเป็นเกี่ยวกับการรักษาพยาบาล");
                htmlTemplate = htmlTemplate.Replace("{ApprovalData.CureMoney}", approvalData.CureMoney.ToString() );

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


        [HttpGet("WarmGenPDF")]
        public async Task<object> WarmGenPDF()
        {
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
            HtmlContent = await GenPDF4WarmUp();

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
            return file;
        }
        private async Task<string> GenPDF4WarmUp()
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
