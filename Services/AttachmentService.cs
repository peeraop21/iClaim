using AutoMapper;
using DataAccess.EFCore.RvpSystemModels;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Nancy.Json;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Services.Models;
using Services.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;


namespace Services
{
    public interface IAttachmentService
    {
        Task<string> DownloadFileFromECM(string documentPath);
        Task<List<EDocDetail>> GetDocumentPath(DocumentDetail model);
        Task<DocumentDetailRes> SaveToEdocDetail(DocumentDetail model);
        Task<ECMRes> UploadFileToECM(ECM model);
        Task<HttpResponseMessage> RequestOcrFrontIdCardAppMan(IFormFile idCardFile);
        Task<HttpResponseMessage> RequestEkycAppMan(EkycReqBody req);
    }
    public class AttachmentService: IAttachmentService
    {
        private readonly IMapper _mapper;
        private readonly RvpSystemContext rvpSystemContext;
        private readonly IConfiguration configuration;


        public AttachmentService(IMapper _mapper, RvpSystemContext rvpSystemContext, IConfiguration configuration)
        {
            this._mapper = _mapper;
            this.rvpSystemContext = rvpSystemContext;
            this.configuration = configuration;

        }
        public async Task<ECMRes> UploadFileToECM(ECM model)
        {
            JObject JsObject = new JObject();
            ECMRes resp = new ECMRes();

            string URL = configuration["API:Attachment:UploadFileToECM"];
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls |
                                       SecurityProtocolType.Tls11 |
                                       SecurityProtocolType.Tls12;
            try
            {
                var httpWebRequest = (HttpWebRequest)WebRequest.Create(URL);
                httpWebRequest.ContentType = "application/json";
                httpWebRequest.Method = "POST";

                using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                {
                    string json = new JavaScriptSerializer().Serialize(new
                    {
                        SystemId = model.SystemId,
                        TemplateId = model.TemplateId,
                        DocID = model.DocID,
                        RefNo = model.RefNo,
                        FileName = model.FileName,
                        Base64String = model.Base64String
                    });

                    streamWriter.Write(json);
                    streamWriter.Flush();
                    streamWriter.Close();
                }


            
                var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    var result = streamReader.ReadToEnd();
                    //JsObject = JObject.Parse(result);


                    resp = JsonConvert.DeserializeObject<ECMRes>(result);


                    //return JsonConvert.SerializeObject(JsObject);
                    return resp;
                }
            }
            catch (HttpRequestException e)
            {
                resp.Path = "Error";
            }
            return null;
        }

        public async Task<string> DownloadFileFromECM(string documentPath)
        {
            JObject JsObject = new JObject();
            var base64Res = "";
            string URL = configuration["API:Attachment:DownloadFileFromECM"];
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls |
                                       SecurityProtocolType.Tls11 |
                                       SecurityProtocolType.Tls12;
            try
            {
                var httpWebRequest = (HttpWebRequest)WebRequest.Create(URL);
                httpWebRequest.ContentType = "application/json";
                httpWebRequest.Method = "POST";

                using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                {
                    string json = new JavaScriptSerializer().Serialize(new
                    {
                        DocumentPath = documentPath
                    });

                    streamWriter.Write(json);
                    streamWriter.Flush();
                    streamWriter.Close();
                }
                var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    var result = streamReader.ReadToEnd();
                    base64Res = JsonConvert.DeserializeObject<string>(result);


                    //return JsonConvert.SerializeObject(JsObject);
                    return base64Res;
                }
            }
            catch (HttpRequestException e)
            {
                return e.Message;
            }
        }

        public async Task<DocumentDetailRes> SaveToEdocDetail(DocumentDetail model)
        {
            if (model is null)
            {
                throw new ArgumentNullException(nameof(model));
            }
            DocumentDetailRes resp = new DocumentDetailRes();
            try
            {
                
                JObject JsObject = new JObject();

                string URL = configuration["API:Attachment:SaveToEdocDetail"];
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls |
                SecurityProtocolType.Tls11 |
                SecurityProtocolType.Tls12;

                ServicePointManager.Expect100Continue = true;

                var httpWebRequest = (HttpWebRequest)WebRequest.Create(URL);
                httpWebRequest.ContentType = "application/json";
                httpWebRequest.Method = "POST";


                using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                {
                    string json = new JavaScriptSerializer().Serialize(new
                    {
                        SystemId = model.SystemId,
                        TemplateID = model.TemplateId,
                        DocumentID = model.DocumentId,
                        RefID = model.RefId,
                        Paths = model.Paths,
                        StatusDoc = model.StatusDoc,
                        Createby = model.Createby

                    });
                    streamWriter.Write(json);
                    streamWriter.Flush();
                    streamWriter.Close();
                }

              
                    var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                    using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                    {
                        var result = streamReader.ReadToEnd();
                        JsObject = JObject.Parse(result);

                        resp = JsonConvert.DeserializeObject<DocumentDetailRes>(result);

                        return resp;
                    }
               
            }
            catch(Exception e)
            {
                resp.Message = e.Message;
                return resp;
            }
                      
        }

        public async Task<List<EDocDetail>> GetDocumentPath(DocumentDetail model)
        {
            var maxRunningNo = await rvpSystemContext.EDocDetail.
                Where(w => w.SystemId == model.SystemId && w.TemplateId == model.TemplateId && w.DocumentId == model.DocumentId && w.RefId.StartsWith(model.RefId))
                .MaxAsync(m => m.RunningNo);          
            var query = await rvpSystemContext.EDocDetail
                .Where(w => w.SystemId == model.SystemId && w.TemplateId == model.TemplateId && w.DocumentId == model.DocumentId && w.RefId.StartsWith(model.RefId) && w.RunningNo == maxRunningNo)
                .Select(s => new EDocDetail(){ Paths = s.Paths, CreateDate = s.CreateDate, RunningNo = s.RunningNo }).OrderByDescending(o => o.RunningNo).ToListAsync();
            return query;
        }

        public async Task<HttpResponseMessage> RequestOcrFrontIdCardAppMan(IFormFile idCardFile)
        {
            string URL = "https://ml.appman.co.th/v1/thailand-id-card/front";
            try
            {
                HttpClient client = new HttpClient();

                client.DefaultRequestHeaders.TryAddWithoutValidation("x-api-key", "uEj0XAy4y69YPgK6IPTFnaVZn7ZsYaum9gJBWowg");
                client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "multipart/form-data");
                using (var multipartFormContent = new MultipartFormDataContent())
                {                    
                    //Add other fields
                    multipartFormContent.Add(new StringContent("Username"), "rvp.user0001");
                    multipartFormContent.Add(new StringContent("Password"), "Rvp@2021");
                    
                    //Add the file
                    var fileStreamContent = new StreamContent(idCardFile.OpenReadStream());
                    fileStreamContent.Headers.ContentType = new MediaTypeHeaderValue("image/png");
                    multipartFormContent.Add(fileStreamContent, "image", idCardFile.FileName);

                    //Send it
                    return  await client.PostAsync(URL, multipartFormContent);
                }
            }
            catch (HttpRequestException e)
            {
                return null;
            }
        }

        public async Task<HttpResponseMessage> RequestEkycAppMan(EkycReqBody req)
        {
            string URL = "https://ml.appman.co.th/mw/e-kyc";

            try
            {
                HttpClient client = new HttpClient();

                client.DefaultRequestHeaders.TryAddWithoutValidation("x-api-key", "uEj0XAy4y69YPgK6IPTFnaVZn7ZsYaum9gJBWowg");
                client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json");

                string message = new JavaScriptSerializer().Serialize(req);
                byte[] messageBytes = System.Text.Encoding.UTF8.GetBytes(message);
                var content = new ByteArrayContent(messageBytes);
                return await client.PostAsync(URL, content);             
            }
            catch (HttpRequestException e)
            {
                return null;
            }
        }
        
    }
}
