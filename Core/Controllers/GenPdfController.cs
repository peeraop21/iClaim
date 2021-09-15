using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using DataAccess.EFCore.DigitalClaimModels;
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
        public GenPdfController(IApprovalService approvalService, IMapper _mapper, IConverter converter)
        {
            this.converter = converter;           
        }
        

        [HttpGet("GetBoto3")]
        public async Task<IActionResult> GetPDF()
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
            HtmlContent = await GenBotoBody();

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

        private async Task<string> GenBotoBody()
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
