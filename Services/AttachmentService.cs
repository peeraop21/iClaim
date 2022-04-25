using AutoMapper;
using DataAccess.EFCore.RvpSystemModels;
using Microsoft.EntityFrameworkCore;
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
using System.Text;
using System.Threading.Tasks;


namespace Services
{
    public interface IAttachmentService
    {
        Task<string> DownloadFileFromECM(string documentPath);
        Task<List<EDocDetail>> GetDocumentPath(EdocDetailViewModel model);
        Task<EdocDetailRes> SaveToEdocDetail(EdocDetailViewModel model);
        Task<ECMViewModelRes> UploadFileToECM(ECMViewModel model);
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
        public async Task<ECMViewModelRes> UploadFileToECM(ECMViewModel model)
        {
            JObject JsObject = new JObject();
            ECMViewModelRes resp = new ECMViewModelRes();

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


                    resp = JsonConvert.DeserializeObject<ECMViewModelRes>(result);


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
                    //JsObject = JObject.Parse(result);


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

        public async Task<EdocDetailRes> SaveToEdocDetail(EdocDetailViewModel model)
        {
            if (model is null)
            {
                throw new ArgumentNullException(nameof(model));
            }
            EdocDetailRes resp = new EdocDetailRes();
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

                        resp = JsonConvert.DeserializeObject<EdocDetailRes>(result);

                        return resp;
                    }
               
            }
            catch(Exception e)
            {
                resp.Message = e.Message;
                return resp;
            }
                      
        }

        public async Task<List<EDocDetail>> GetDocumentPath(EdocDetailViewModel model)
        {
            var maxRunningNo = await rvpSystemContext.EDocDetail.
                Where(w => w.SystemId == model.SystemId && w.TemplateId == model.TemplateId && w.DocumentId == model.DocumentId && w.RefId.StartsWith(model.RefId))
                .MaxAsync(m => m.RunningNo);          
            var query = await rvpSystemContext.EDocDetail
                .Where(w => w.SystemId == model.SystemId && w.TemplateId == model.TemplateId && w.DocumentId == model.DocumentId && w.RefId.StartsWith(model.RefId) && w.RunningNo == maxRunningNo)
                .Select(s => new EDocDetail(){ Paths = s.Paths, CreateDate = s.CreateDate, RunningNo = s.RunningNo }).OrderByDescending(o => o.RunningNo).ToListAsync();
            return query;
        }
        
    }
}
