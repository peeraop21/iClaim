using AutoMapper;
using Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Nancy.Json;
using Services;
using Services.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;


namespace Core.Controllers
{
    [EnableCors("iClaim")]
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IUserService userService;
        private readonly IMasterService masterService;
        private readonly IAttachmentService attachmentService;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly ILogger<UserController> logger;
        private readonly IConfiguration configuration;


        public UserController(IMapper _mapper, IUserService userService, IMasterService masterService, IAttachmentService attachmentService, IHttpContextAccessor httpContextAccessor, ILogger<UserController> logger, IConfiguration configuration)
        {
            this._mapper = _mapper;
            this.userService = userService;
            this.masterService = masterService;
            this.attachmentService = attachmentService;
            this.httpContextAccessor = httpContextAccessor;
            this.logger = logger;   
            this.configuration = configuration;
        }
        

        [Authorize]
        [HttpPost("GetUser")]
        public async Task<IActionResult> GetUser([FromBody] ReqData req)
        {
            try
            {
                return Ok(await userService.GetUserByIdLine(req.UserIdLine));
            }
            catch (Exception ex)
            {
                string baseUrl = configuration["BaseUrl:Publish"];
                if (!string.IsNullOrEmpty(req.UserIdLine))
                {
                    logger.LogError(baseUrl + ", API: GetUser, User: {0}, Exception: {1}", req.UserIdLine, ex);
                    return StatusCode(500);
                }
                else
                {
                    logger.LogError(baseUrl + ", API: GetUser, User: {0}, Exception: {1}", req.UserIdCard, ex);
                    return StatusCode(500);
                }
            }
            
        }

        [HttpPost("CheckRegister")]
        public async Task<IActionResult> CheckRegister([FromBody] ReqData req)
        {
            try
            {
                return Ok(await userService.CheckRegisterByIdLine(req.UserIdLine));
            }
            catch (Exception ex)
            {
                string baseUrl = configuration["BaseUrl:Publish"];
                if (!string.IsNullOrEmpty(req.UserIdLine))
                {
                    logger.LogError(baseUrl + ", API: CheckRegister, User: {0}, Exception: {1}", req.UserIdLine, ex);
                    return StatusCode(500);
                }
                else
                {
                    logger.LogError(baseUrl + ", API: CheckRegister, User: {0}, Exception: {1}", req.UserIdCard, ex);
                    return StatusCode(500);
                }
            }
            
        }

        [HttpPost]
        public async Task<IActionResult> PostAsync([FromBody] User model)
        {
            try
            {
                var ip = httpContextAccessor.HttpContext.Connection.RemoteIpAddress.ToString();
                var kycno = await userService.GetLastKyc();
                //var genAddress = await masterService.GetIdAddress(model.HomeProvinceId, model.HomeCityId, model.HomeTumbolId);
                //model.HomeProvinceId = genAddress.ProvinceId;
                //model.HomeCityId = genAddress.DistrictId;
                //model.HomeTumbolId = genAddress.SubDistrictId;
                //model.HomeZipcode = genAddress.ZipCode;
                model.IdcardNo = model.IdcardNo.Replace("-", "");
                model.MobileNo = model.MobileNo.Replace("-", "");
                model.IdcardLaserCode = model.IdcardLaserCode.Replace("-", "");
                model.DateofBirth = DateTime.ParseExact(model.StringDateofBirth, "yyyy-MM-dd", CultureInfo.InvariantCulture);
                model.Kycno = kycno + 1;
                model.IdcardNo = model.IdcardNo.Replace(" ", "");
                model.InsertDate = DateTime.Now;
                model.LastUpdateDate = DateTime.Now;
                model.Status = "Y";
                model.LastUpdateIp = ip;
                model.LineRegisDate = DateTime.Now;
                model.LineRegisIp = ip;


                ECM idCardEcmModel = new ECM();
                idCardEcmModel.SystemId = "02";
                idCardEcmModel.TemplateId = "03";
                idCardEcmModel.DocID = "04";
                idCardEcmModel.RefNo = model.LineId + "|" + model.IdcardNo;
                idCardEcmModel.FileName = model.Kycno + "IdCard" + model.IdcardNo + ".png";
                idCardEcmModel.Base64String = model.Base64IdCard;
                var idCardEcmRes = await attachmentService.UploadFileToECM(idCardEcmModel);
                var idCardResultMapEdocDetail = _mapper.Map<DocumentDetail>(idCardEcmModel);
                idCardResultMapEdocDetail.Paths = idCardEcmRes.Path;
                await attachmentService.SaveToEdocDetail(idCardResultMapEdocDetail);

                ECM faceEcmModel = new ECM();
                faceEcmModel.SystemId = "02";
                faceEcmModel.TemplateId = "03";
                faceEcmModel.DocID = "05";
                faceEcmModel.RefNo = model.LineId + "|" + model.IdcardNo;
                faceEcmModel.FileName = model.Kycno + "Face" + model.IdcardNo + ".png";
                faceEcmModel.Base64String = model.Base64Face;
                var faceEcmRes = await attachmentService.UploadFileToECM(faceEcmModel);
                var faceResultMapEdocDetail = _mapper.Map<DocumentDetail>(faceEcmModel);
                faceResultMapEdocDetail.Paths = faceEcmRes.Path;
                await attachmentService.SaveToEdocDetail(faceResultMapEdocDetail);

                return Ok(await userService.AddAsync(model));
            }
            catch (Exception ex)
            {
                string baseUrl = configuration["BaseUrl:Publish"];
                if (!string.IsNullOrEmpty(model.LineId))
                {
                    logger.LogError(baseUrl + ", API: PostUser, User: {0}, Exception: {1}", model.LineId, ex);
                    return StatusCode(500);
                }
                else
                {
                    logger.LogError(baseUrl + ", API: PostUser, User: {0}, Exception: {1}", model.IdcardNo, ex);
                    return StatusCode(500);
                }
            }
            
        }


        [HttpPost("Ocr")]
        public async Task<IActionResult> OcrFrontIdCard(IFormFile file)
        {
            try
            {
                var response = await attachmentService.RequestOcrFrontIdCardAppMan(file);
                var read_response = response.Content.ReadAsStringAsync().Result;
                var result = new JavaScriptSerializer().DeserializeObject(read_response);
                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    return Ok(result);
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.Accepted)
                {

                    return StatusCode(202, result);
                }
                else
                {
                    return BadRequest();
                }
            }catch (Exception ex)
            {
                string baseUrl = configuration["BaseUrl:Publish"];
                logger.LogError(baseUrl + ", API: OcrFrontIdCard, Exception: {0}", ex);
                return StatusCode(500);
            }
           
             
        }

        [HttpPost("Ekyc")]
        public async Task<IActionResult> Ekyc([FromBody] ReqEkyc req)
        {
            try
            {
                var response = await attachmentService.RequestEkycAppMan(_mapper.Map<EkycReqBody>(req));
                var read_response = response.Content.ReadAsStringAsync().Result;
                var result = new JavaScriptSerializer().DeserializeObject(read_response);
                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    return Ok(result);
                }
                else
                {
                    return BadRequest();
                }
            }
            catch (Exception ex)
            {
                string baseUrl = configuration["BaseUrl:Publish"];
                logger.LogError(baseUrl + ", API: Ekyc, Exception: {0}", ex);
                return StatusCode(500);
            }
           
        }
    }
}
