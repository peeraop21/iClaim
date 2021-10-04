using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using AutoMapper;
using Core.ViewModels;
using DataAccess.EFCore.DigitalClaimModels;
using DinkToPdf;
using DinkToPdf.Contracts;
using iTextSharp.text;
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
        public GenPdfController( IConverter converter, IAccidentService accidentService)
        {
            this.converter = converter;
            this.accidentService = accidentService;
        }
        

        //[HttpGet("GetBoto3/{accNo}/{victimNo}/{appNo}/{channel}")]
        [HttpPost]
        public async Task<IActionResult> GetPDF([FromBody]PostDataViewModel model)
        {
            var acc = await accidentService.GetAccidentForGenPDF(model.AccNo.Replace("-", "/"), model.VictimNo, model.AppNo);
            var accVictim = await accidentService.GetAccidentVictim(model.AccNo.Replace("-", "/"), model.Channel, "", model.VictimNo);
            var accCar = await accidentService.GetAccidentCar(model.AccNo.Replace("-", "/"), model.Channel);

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
            HtmlContent = await GenBotoBody(acc, accVictim, accCar);

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

        private async Task<string> GenBotoBody(AccidentPDFViewModel acc, VictimtViewModel accVictim, CarViewModel accCar)
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




                return htmlTemplate;
            }
        }

        


    }
}
