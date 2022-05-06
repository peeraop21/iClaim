using AutoMapper;
using Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Nancy.Json;
using Services;
using Services.Models;
using Services.ViewModels;
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


        public UserController(IMapper _mapper, IUserService userService, IMasterService masterService, IAttachmentService attachmentService, IHttpContextAccessor httpContextAccessor)
        {
            this._mapper = _mapper;
            this.userService = userService;
            this.masterService = masterService;
            this.attachmentService = attachmentService;
            this.httpContextAccessor = httpContextAccessor;
        }
        

        [Authorize]
        [HttpPost("GetUser")]
        public async Task<IActionResult> GetUser([FromBody] ReqData req)
        {
            return Ok(await userService.GetUserByIdLine(req.UserIdLine));
        }

        [HttpPost("CheckRegister")]
        public async Task<IActionResult> CheckRegister([FromBody] ReqData req)
        {
            return Ok(await userService.CheckRegisterByIdLine(req.UserIdLine));
        }

        [HttpPost]
        public async Task<IActionResult> PostAsync([FromBody] DirectPolicyKycViewModel model)
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


            ECMViewModel idCardEcmModel = new ECMViewModel();
            idCardEcmModel.SystemId = "02";
            idCardEcmModel.TemplateId = "03";
            idCardEcmModel.DocID = "04";
            idCardEcmModel.RefNo = model.LineId + "|" + model.IdcardNo;
            idCardEcmModel.FileName = model.Kycno + "IdCard" + model.IdcardNo + ".png";
            idCardEcmModel.Base64String = model.Base64IdCard;
            var idCardEcmRes = await attachmentService.UploadFileToECM(idCardEcmModel);
            var idCardResultMapEdocDetail = _mapper.Map<EdocDetailViewModel>(idCardEcmModel);
            idCardResultMapEdocDetail.Paths = idCardEcmRes.Path;
            await attachmentService.SaveToEdocDetail(idCardResultMapEdocDetail);

            ECMViewModel faceEcmModel = new ECMViewModel();
            faceEcmModel.SystemId = "02";
            faceEcmModel.TemplateId = "03";
            faceEcmModel.DocID = "05";
            faceEcmModel.RefNo = model.LineId + "|" + model.IdcardNo;
            faceEcmModel.FileName = model.Kycno + "Face" + model.IdcardNo + ".png";
            faceEcmModel.Base64String = model.Base64Face;
            var faceEcmRes = await attachmentService.UploadFileToECM(faceEcmModel);
            var faceResultMapEdocDetail = _mapper.Map<EdocDetailViewModel>(faceEcmModel);
            faceResultMapEdocDetail.Paths = faceEcmRes.Path;
            await attachmentService.SaveToEdocDetail(faceResultMapEdocDetail);

            return Ok(await userService.AddAsync(model));
        }


        [HttpPost("Ocr")]
        public async Task<IActionResult> OcrFrontIdCard(IFormFile file)
        {
            var response = await attachmentService.RequestOcrFrontIdCardAppMan(file);
            var read_response = response.Content.ReadAsStringAsync().Result;
            var result = new JavaScriptSerializer().DeserializeObject(read_response);
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return Ok(result);
            }else if (response.StatusCode == System.Net.HttpStatusCode.Accepted)
            {

                return StatusCode(202, result);
            }
            else
            {
                return BadRequest();
            }
             
        }

        [HttpPost("Ekyc")]
        public async Task<IActionResult> Ekyc([FromBody] ReqEkyc req)
        {
            var response = await attachmentService.RequestEkycAppMan(_mapper.Map<EkycReqBody>(req));
            var read_response = response.Content.ReadAsStringAsync().Result;
            var result = new JavaScriptSerializer().DeserializeObject(read_response);
            if(response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return Ok(result);
            }
            else
            {
                return BadRequest();
            }
        }
    }
}
