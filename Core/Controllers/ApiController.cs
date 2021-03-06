using Core.Models.API;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Nancy.Json;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Services.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace Core.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ApiController : ControllerBase
    {
        private readonly IConfiguration configuration;

        public ApiController(IConfiguration configuration)
        {
            this.configuration = configuration;
        }
        [HttpPost("PushMessage")]
        public async Task<IActionResult> PushMessage([FromBody] PushMessageReq models)
        {
            var baseUrl = configuration["BaseUrl:Publish"];

            string URL = "https://api.line.me/v2/bot/message/push";
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls |
                                       SecurityProtocolType.Tls11 |
                                       SecurityProtocolType.Tls12;

            var resp = new object();
            try
            {
                var httpWebRequest = (HttpWebRequest)WebRequest.Create(URL);
                httpWebRequest.ContentType = "application/json";
                httpWebRequest.Method = "POST";
                httpWebRequest.Headers.Add("Authorization", "Bearer NndiohliHYK/CUvBE60RXaiCLh2Fe5LvLrnxUJCsbnVZiN42IBbL8aqbkLptcQ/lZ27Jtp5RADGnJrusgBYpDVYNWjzi4PhIkli4qpPqTBHcTTX0j/dWCeBbz74y0iJNaXm/E1k32p1nPLjmMbIdxwdB04t89/1O/w1cDnyilFU=");

                using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                {
                    if (models.Type.Equals("Text"))
                    {
                        string json = new JavaScriptSerializer().Serialize(new
                        {
                            to = models.To,
                            messages = new[]{
                                new LineMessage{ Type = "text", Text = models.Messages },
                            }
                        });
                        streamWriter.Write(json);
                        streamWriter.Flush();
                        streamWriter.Close();
                    }
                    else if (models.Type.Equals("Flex"))
                    {
                        string json = new JavaScriptSerializer().Serialize(new
                        {
                            to = models.To,
                            messages = new[]{
                                new {
                                        Type = "flex",
                                        AltText = "this is a flex message",
                                        Contents = new
                                        {
                                            
                                                Type = "bubble",
                                                Body = new
                                                {
                                                    Type = "box",
                                                    Layout = "horizontal",
                                                    Contents = new[]
                                                    {
                                                        new
                                                        {
                                                            Type="text",
                                                            Text= models.Messages,
                                                            Wrap = true
                                                        }
                                                    }
                                                },
                                                Footer = new
                                                {
                                                    Type = "box",
                                                    Layout = "horizontal",
                                                    Contents = new[]
                                                    {
                                                        new
                                                        {
                                                            Type="button",
                                                            Style= "primary",
                                                            Action = new
                                                            {
                                                                Type = "uri",
                                                                Label = "เข้าสู่ระบบ",
                                                                Uri = baseUrl + "?openExternalBrowser=1"
                                                            }
                                                        }
                                                    }
                                                }                                           
                                        }
                                    },
                            }
                        });
                        streamWriter.Write(json);
                        streamWriter.Flush();
                        streamWriter.Close();
                    }
                }
                var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    var result = streamReader.ReadToEnd();
                    resp = JsonConvert.DeserializeObject<object>(result);
                }
            }
            catch (HttpRequestException e)
            {
                resp = "Error";
            }
            return Ok(resp);
        }

        
    }
}
