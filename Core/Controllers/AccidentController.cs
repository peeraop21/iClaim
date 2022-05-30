
using AutoMapper;
using Core.Models;
using DataAccess.EFCore.RvpOfficeModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services;
using Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Core.Controllers
{
    [EnableCors("iClaim")]
    [ApiController]
    [Route("api/[controller]")]
    public class AccidentController : ControllerBase
    {
        private readonly IAccidentService accidentService;
        private readonly IMasterService masterService;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly IAttachmentService attachmentService;

        public AccidentController(IAccidentService accidentService, IMasterService masterService, IMapper _mapper, IHttpContextAccessor httpContextAccessor, IAttachmentService attachmentService) {
            this.accidentService = accidentService;
            this.masterService = masterService;
            this._mapper = _mapper;
            this.httpContextAccessor = httpContextAccessor;
            this.attachmentService = attachmentService;
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> GetAccident([FromBody] ReqData req)
        {
            return Ok(new {
                AccData = await accidentService.GetAccidentByIdLine(req.UserIdLine),
                PolicyCount = await accidentService.GetEpolicyCountAsync(req.UserIdCard)
            });
        }

        [Authorize]
        [HttpPost("Car")]
        public async Task<IActionResult> GetAccidentCar([FromBody] ReqData req)
        {
            return Ok(await accidentService.GetAccidentCar(req.AccNo.Replace("-","/")));
        }

        [Authorize]
        [HttpPost("Victim")]
        public async Task<IActionResult> GetAccidentVictim([FromBody] ReqData req)
        {
            return Ok(await accidentService.GetAccidentVictim(req.AccNo.Replace("-", "/"), req.UserIdCard, req.VictimNo));
        }

        [HttpPost("DataForAccidentCreatePage")]
        public async Task<IActionResult> GetDataForAccidentCreatePage([FromBody] ReqData req)
        {
            return Ok(new
            {
                Provinces = await masterService.GetChangwatsAsync(),
                Cars = await accidentService.GetEpoliciesByIdCardAsync(req.UserIdCard),
                CurrentAddress = await accidentService.GetLastCurrentAddressByIdCardNo(req.UserIdCard)
            });
        }

        [HttpPost("InsertAccident")]
        public async Task<IActionResult> PostAccident([FromBody] ReqPostAccident req)
        {
            try
            {
                var ip = httpContextAccessor.HttpContext.Connection.RemoteIpAddress.ToString();
                var hosAccident = _mapper.Map<HosAccident>(req.AccidentInput);
                var hosCarAccident = _mapper.Map<HosCarAccident>(req.AccidentCarInput);
                var hosVicTimAccident = _mapper.Map<HosVicTimAccident>(req.AccidentVictimInput);

                var result = await accidentService.AddAsync(hosAccident, hosCarAccident, hosVicTimAccident, ip);

                /////// รอพี่เหลียง config
                //if(!string.IsNullOrEmpty(result))
                //{
                //    for (int i = 0; i < req.AccidentInput.AccImages.Count; i++)
                //    {
                //        ECM ecmModel = new ECM();
                //        ecmModel.SystemId = "02";
                //        ecmModel.TemplateId = "03";
                //        ecmModel.DocID = "07";

                //        ecmModel.RefNo = result + "-" + "Acc" + "-" + (i + 1);
                //        ecmModel.FileName = req.AccidentInput.AccImages[i].Filename;
                //        ecmModel.Base64String = req.AccidentInput.AccImages[i].Base64;
                //        var accImgRes = await attachmentService.UploadFileToECM(ecmModel);
                //        var resultMapEdocDetail = _mapper.Map<DocumentDetail>(ecmModel);
                //        resultMapEdocDetail.Paths = accImgRes.Path;
                //        await attachmentService.SaveToEdocDetail(resultMapEdocDetail);
                //    }
                //    for (int i = 0; i < req.AccidentCarInput.AccCarImages.Count; i++)
                //    {
                //        ECM ecmModel = new ECM();
                //        ecmModel.SystemId = "02";
                //        ecmModel.TemplateId = "03";
                //        ecmModel.DocID = "08";

                //        ecmModel.RefNo = result + "-" + "AccCar" + "-" + (i + 1);
                //        ecmModel.FileName = req.AccidentCarInput.AccCarImages[i].Filename;
                //        ecmModel.Base64String = req.AccidentCarInput.AccCarImages[i].Base64;
                //        var accCarImgRes = await attachmentService.UploadFileToECM(ecmModel);
                //        var resultMapEdocDetail = _mapper.Map<DocumentDetail>(ecmModel);
                //        resultMapEdocDetail.Paths = accCarImgRes.Path;
                //        await attachmentService.SaveToEdocDetail(resultMapEdocDetail);
                //    }
                //    for (int i = 0; i < req.AccidentVictimInput.AccVicBrokenImages.Count; i++)
                //    {
                //        ECM ecmModel = new ECM();
                //        ecmModel.SystemId = "02";
                //        ecmModel.TemplateId = "03";
                //        ecmModel.DocID = "09";

                //        ecmModel.RefNo = result + "-" + "AccVic" + "-" + (i + 1);
                //        ecmModel.FileName = req.AccidentVictimInput.AccVicBrokenImages[i].Filename;
                //        ecmModel.Base64String = req.AccidentVictimInput.AccVicBrokenImages[i].Base64;
                //        var accVicImgRes = await attachmentService.UploadFileToECM(ecmModel);
                //        var resultMapEdocDetail = _mapper.Map<DocumentDetail>(ecmModel);
                //        resultMapEdocDetail.Paths = accVicImgRes.Path;
                //        await attachmentService.SaveToEdocDetail(resultMapEdocDetail);
                //    }
                //}

                return Ok(new
                {
                    Status = "Success",
                    Messages = "",

                });
            }catch (Exception ex)
            {
                return Ok(new
                {
                    Status = "Error",
                    Messages = ex.Message,
                });
            }
            
        }

        [HttpPost("DataForAccidentEditPage")]
        public async Task<IActionResult> GetDataForAccidentEditPage([FromBody] ReqData req)
        {

            return Ok();
        }
    }
}
