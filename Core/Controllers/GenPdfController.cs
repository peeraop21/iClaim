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
            var acc = await accidentService.GetAccidentForGenPDF(model.AccNo.Replace("-", "/"), model.VictimNo, model.AppNo);
            var accVictim = await accidentService.GetAccidentVictim(model.AccNo.Replace("-", "/"), model.Channel, model.UserIdCard, model.VictimNo);
            var accCar = await accidentService.GetAccidentCar(model.AccNo.Replace("-", "/"), model.Channel);
            var approvalData = await approvalService.GetApprovalDataForGenPDF(model.AccNo.Replace("-", "/"), model.VictimNo, model.AppNo);

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
            //var path = @"C:\Users\ROIN\source\repos\iRVP\Digital Claim\Core\license.key";
            //try
            //{
            //    HashAlgorithm hashAlgorithm = HashAlgorithm.SHA256;
            //    Rectangle signatureRectCert = new Rectangle(100, 100, 400, 800);
            //    signatureRectCert = new Rectangle(0, 0, 0, 0);
            //    Font signatureFont = new Font(Font.FontFamily.COURIER, 7.0f, Font.NORMAL, BaseColor.BLACK);
            //    Pkcs11CsWrapper pks = new Pkcs11CsWrapper(NShieldLibraryPath, useOsLocking, UserPIN, TokenSerial, TokenLabel, path);


            //    string subject = "Accident No : " + acc.AccNo;
            //    byte[] contentByte = Encoding.UTF8.GetBytes(subject);
            //    byte[] contentHash = pks.signContent(contentByte);
            //    string contentBase64 = Convert.ToBase64String(contentHash);

            //    pks.initPkcs11RsaSignature(ckaLabel, null, hashAlgorithm, certificateThumbprint, onlyValidCertificate);
            //    byte[] fileSigned = pks.signPdf(file, signingBlockName, contentBase64, signatureRectCert, signatureFont);
            //}
            //catch(Exception ex)
            //{
            //    var e = ex;
            //}
            return File(file, "application/pdf");
        }

        private async Task<string> GenBotoBody(AccidentPDFViewModel acc, VictimtViewModel accVictim, CarViewModel accCar, ApprovalPDFViewModel approvalData)
        {
            var template = System.IO.Directory.GetCurrentDirectory() + @"\Templates\Boto3_Template.html";
            using (StreamReader reader = new StreamReader(template))
            {
                var htmlTemplate = await reader.ReadToEndAsync();
                htmlTemplate = htmlTemplate.Replace("{AccNo}", (string.IsNullOrEmpty(acc.AccNo)) ? "-" : acc.AccNo);
                htmlTemplate = htmlTemplate.Replace("{AccVictim.Name}", (string.IsNullOrEmpty(accVictim.Fname)) ? "-" : accVictim.Prefix + accVictim.Fname + " " + accVictim.Lname);
                htmlTemplate = htmlTemplate.Replace("( ) ผู้ประสบภัย", "(X) ผู้ประสบภัย");
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
                htmlTemplate = htmlTemplate.Replace("{Acc.Place}", acc.AccPlace);
                htmlTemplate = htmlTemplate.Replace("{AccCar.FoundCarLicense}", (string.IsNullOrEmpty(accCar.FoundCarLicense)) ? "-" : accCar.FoundCarLicense);
                htmlTemplate = htmlTemplate.Replace("{AccCar.FoundChassisNo}", (string.IsNullOrEmpty(accCar.FoundChassisNo)) ? "-" : accCar.FoundChassisNo);
                htmlTemplate = htmlTemplate.Replace("{AccCar.FoundPolicyNo}", (string.IsNullOrEmpty(accCar.FoundPolicyNo)) ? "-" : accCar.FoundPolicyNo);
                htmlTemplate = htmlTemplate.Replace("( ) รถคันเดียว ไม่มีคู่กรณี", "(X) รถคันเดียว ไม่มีคู่กรณี");
                if (accVictim.VictimIs == "ผขป")
                {
                    htmlTemplate = htmlTemplate.Replace("( ) ผู้ขับขี่", "(X) ผู้ขับขี่");
                }else if (accVictim.VictimIs == "ผสป")
                {
                    htmlTemplate = htmlTemplate.Replace("( ) ผู้โดยสารรถคันเอาประกันภัย", "(X) ผู้โดยสารรถคันเอาประกันภัย");
                }
                if (accVictim.VictimType == "IPD")
                {
                    htmlTemplate = htmlTemplate.Replace("( ) ผู้ป่วยใน", "(X) ผู้ป่วยใน");
                }else
                {
                    htmlTemplate = htmlTemplate.Replace("( ) ผู้ป่วยนอก", "(X) ผู้ป่วยนอก");
                }
                






                return htmlTemplate;
            }
        }

        


    }
}
