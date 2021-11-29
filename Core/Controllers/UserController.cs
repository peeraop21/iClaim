using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services;
using Services.ViewModels;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Core.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IUserService userService;
        private readonly IMasterService masterService;
        private readonly IAttachmentService attachmentService;


        public UserController(IMapper _mapper, IUserService userService, IMasterService masterService, IAttachmentService attachmentService)
        {
            this._mapper = _mapper;
            this.userService = userService;
            this.masterService = masterService;
            this.attachmentService = attachmentService;
        }
        

        [Authorize]
        // GET api/<UserController>/5
        [HttpGet("{userToken}")]
        public async Task<IActionResult> Get(string userToken)
        {
            return Ok(await userService.GetUser(userToken));
        }

        [Authorize]
        // GET api/<UserController>/5
        [HttpGet("CheckRegister/{userToken}")]
        public async Task<IActionResult> CheckRegister(string userToken)
        {
            return Ok(await userService.CheckRegister(userToken));
        }

        [HttpPost]
        public async Task<IActionResult> PostAsync([FromBody] DirectPolicyKycViewModel model)
        {
            var kycno = await userService.GetLastKyc();
            //var genAddress = await masterService.GetIdAddress(model.HomeProvinceId, model.HomeCityId, model.HomeTumbolId);
            //model.HomeProvinceId = genAddress.ProvinceId;
            //model.HomeCityId = genAddress.DistrictId;
            //model.HomeTumbolId = genAddress.SubDistrictId;
            //model.HomeZipcode = genAddress.ZipCode;
            model.DateofBirth = DateTime.ParseExact(model.StringDateofBirth, "yyyy-MM-dd", CultureInfo.InvariantCulture);
            model.Kycno = kycno + 1;
            model.IdcardNo = model.IdcardNo.Replace(" ", "");
            model.InsertDate = DateTime.Now;
            model.LastUpdateDate = DateTime.Now;

            ECMViewModel ecmModel = new ECMViewModel();
            ecmModel.SystemId = "02";
            ecmModel.TemplateId = "03";
            ecmModel.DocID = "04";
            ecmModel.RefNo = model.LineId + "|" + model.IdcardNo;
            ecmModel.FileName = model.IdcardNo + ".png";
            ecmModel.Base64String = model.Base64IdCard;
            var idCardEcmRes = await attachmentService.UploadFileToECM(ecmModel);
            var idCardResultMapEdocDetail = _mapper.Map<EdocDetailViewModel>(ecmModel);
            idCardResultMapEdocDetail.Paths = idCardEcmRes.Path;
            await attachmentService.SaveToEdocDetail(idCardResultMapEdocDetail);
            return Ok(await userService.AddAsync(model));
        }
    }
}
