
using AutoMapper;
using Core.Models;
using DataAccess.EFCore.RvpOfficeModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
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
        private readonly ILogger<AccidentController> logger;
        private readonly IConfiguration configuration;
        
        public AccidentController(IAccidentService accidentService, IMasterService masterService, IMapper _mapper, IHttpContextAccessor httpContextAccessor, IAttachmentService attachmentService, ILogger<AccidentController> logger, IConfiguration configuration) {
            this.accidentService = accidentService;
            this.masterService = masterService;
            this._mapper = _mapper;
            this.httpContextAccessor = httpContextAccessor;
            this.attachmentService = attachmentService;
            this.logger = logger;
            this.configuration = configuration;
            
        }
        
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> GetAccident([FromBody] ReqData req)
        {
            try
            {
                return Ok(new
                {
                    AccData = await accidentService.GetAccidentByIdLine(req.UserIdLine),
                    PolicyCount = await accidentService.GetEpolicyCountAsync(req.UserIdCard)
                });
            }
            catch (Exception ex)
            {
                string baseUrl = configuration["BaseUrl:Publish"];
                if (!string.IsNullOrEmpty(req.UserIdLine))
                {
                    logger.LogError(baseUrl + ", API: GetAccident, User: {0}, Exception: {1}", req.UserIdLine, ex);
                    return StatusCode(500);
                }
                else
                {
                    logger.LogError(baseUrl + ", API: GetAccident, User: {0}, Exception: {1}", req.UserIdCard, ex);
                    return StatusCode(500);
                }
                
            }
           
        }

        [Authorize]
        [HttpPost("Car")]
        public async Task<IActionResult> GetAccidentCar([FromBody] ReqData req)
        {
            try
            {
                return Ok(await accidentService.GetAccidentCar(req.AccNo.Replace("-", "/")));
            }
            catch (Exception ex)
            {
                string baseUrl = configuration["BaseUrl:Publish"];
                if (!string.IsNullOrEmpty(req.UserIdLine))
                {
                    logger.LogError(baseUrl + ", API: GetAccidentCar, User: {0}, Exception: {1}", req.UserIdLine, ex);
                    return StatusCode(500);
                }
                else
                {
                    logger.LogError(baseUrl + ", API: GetAccidentCar, User: {0}, Exception: {1}", req.UserIdCard, ex);
                    return StatusCode(500);
                }
                
            }
            
        }

        [Authorize]
        [HttpPost("Victim")]
        public async Task<IActionResult> GetAccidentVictim([FromBody] ReqData req)
        {
            try
            {
                return Ok(await accidentService.GetAccidentVictim(req.AccNo.Replace("-", "/"), req.UserIdCard, req.VictimNo));
            }
            catch (Exception ex)
            {
                string baseUrl = configuration["BaseUrl:Publish"];
                if (!string.IsNullOrEmpty(req.UserIdLine))
                {
                    logger.LogError(baseUrl + ", API: GetAccidentVictim, User: {0}, Exception: {1}", req.UserIdLine, ex);
                    return StatusCode(500);
                }
                else
                {
                    logger.LogError(baseUrl + ", API: GetAccidentVictim, User: {0}, Exception: {1}", req.UserIdCard, ex);
                    return StatusCode(500);
                }
            }
            
        }

        [HttpPost("DataForAccidentCreatePage")]
        public async Task<IActionResult> GetDataForAccidentCreatePage([FromBody] ReqData req)
        {
            try
            {
                return Ok(new
                {
                    Provinces = await masterService.GetChangwatsAsync(),
                    Cars = await accidentService.GetEpoliciesByIdCardAsync(req.UserIdCard),
                    CurrentAddress = await accidentService.GetLastCurrentAddressByIdCardNo(req.UserIdCard)
                });
            }catch (Exception ex)
            {
                string baseUrl = configuration["BaseUrl:Publish"];
                if (!string.IsNullOrEmpty(req.UserIdLine))
                {
                    logger.LogError(baseUrl + ", API: GetDataForAccidentCreatePage, User: {0}, Exception: {1}", req.UserIdLine, ex);
                    return StatusCode(500);
                }
                else
                {
                    logger.LogError(baseUrl + ", API: GetDataForAccidentCreatePage, User: {0}, Exception: {1}", req.UserIdCard, ex);
                    return StatusCode(500);
                }
            }
            
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
                if (!string.IsNullOrEmpty(result))
                {
                    for (int i = 0; i < req.AccidentInput.AccImages.Count; i++)
                    {
                        string extensionFile = await masterService.GetExtensionFile(req.AccidentInput.AccImages[i].Filename);
                        ECM ecmModel = new ECM();
                        ecmModel.SystemId = "02";
                        ecmModel.TemplateId = "03";
                        ecmModel.DocID = "07";

                        ecmModel.RefNo = result + "-" + (i + 1);
                        ecmModel.FileName = result.Replace("/","") + "_" + "Acc" + "_" + (i + 1) + "." + extensionFile;
                        ecmModel.Base64String = req.AccidentInput.AccImages[i].Base64;
                        var accImgRes = await attachmentService.UploadFileToECM(ecmModel);
                        var resultMapEdocDetail = _mapper.Map<DocumentDetail>(ecmModel);
                        resultMapEdocDetail.Paths = accImgRes.Path;
                        await attachmentService.SaveToEdocDetail(resultMapEdocDetail);
                    }
                    for (int i = 0; i < req.AccidentCarInput.AccCarImages.Count; i++)
                    {
                        string extensionFile = await masterService.GetExtensionFile(req.AccidentCarInput.AccCarImages[i].Filename);
                        ECM ecmModel = new ECM();
                        ecmModel.SystemId = "02";
                        ecmModel.TemplateId = "03";
                        ecmModel.DocID = "08";

                        ecmModel.RefNo = result + "-" + (i + 1);
                        ecmModel.FileName = result.Replace("/", "") + "_" + "AccCar" + "_" + (i + 1) + "." + extensionFile;
                        ecmModel.Base64String = req.AccidentCarInput.AccCarImages[i].Base64;
                        var accCarImgRes = await attachmentService.UploadFileToECM(ecmModel);
                        var resultMapEdocDetail = _mapper.Map<DocumentDetail>(ecmModel);
                        resultMapEdocDetail.Paths = accCarImgRes.Path;
                        await attachmentService.SaveToEdocDetail(resultMapEdocDetail);
                    }
                    for (int i = 0; i < req.AccidentVictimInput.AccVicBrokenImages.Count; i++)
                    {
                        string extensionFile = await masterService.GetExtensionFile(req.AccidentVictimInput.AccVicBrokenImages[i].Filename);
                        ECM ecmModel = new ECM();
                        ecmModel.SystemId = "02";
                        ecmModel.TemplateId = "03";
                        ecmModel.DocID = "09";

                        ecmModel.RefNo = result + "-" + (i + 1);
                        ecmModel.FileName = result.Replace("/", "") + "_" + "AccVictim" + "_" + (i + 1) + "." + extensionFile;
                        ecmModel.Base64String = req.AccidentVictimInput.AccVicBrokenImages[i].Base64;
                        var accVicImgRes = await attachmentService.UploadFileToECM(ecmModel);
                        var resultMapEdocDetail = _mapper.Map<DocumentDetail>(ecmModel);
                        resultMapEdocDetail.Paths = accVicImgRes.Path;
                        await attachmentService.SaveToEdocDetail(resultMapEdocDetail);
                    }
                }

                return Ok(new
                {
                    Status = "Success",
                    Messages = "",

                });
            }catch (Exception ex)
            {
                string baseUrl = configuration["BaseUrl:Publish"];
                if (!string.IsNullOrEmpty(req.AccidentVictimInput.AccVicUserLineId))
                {
                    logger.LogError(baseUrl + ", API: PostAccident, User: {0}, Exception: {1}", req.AccidentVictimInput.AccVicUserLineId, ex);
                    return StatusCode(500);
                }
                else
                {
                    logger.LogError(baseUrl + ", API: PostAccident, User: {0}, Exception: {1}", req.AccidentVictimInput.AccVicIdCardNo, ex);
                    return StatusCode(500);
                }
            }
            
        }

        [HttpPost("DataForAccidentEditPage")]
        public async Task<IActionResult> GetDataForAccidentEditPage([FromBody] ReqData req)
        {
            try
            {

                return Ok();
            }
            catch (Exception ex)
            {
                string baseUrl = configuration["BaseUrl:Publish"];
                if (!string.IsNullOrEmpty(req.UserIdLine))
                {
                    logger.LogError(baseUrl + ", API: GetDataForAccidentEditPage, User: {0}, Exception: {1}", req.UserIdLine, ex);
                    return StatusCode(500);
                }
                else
                {
                    logger.LogError(baseUrl + ", API: GetDataForAccidentEditPage, User: {0}, Exception: {1}", req.UserIdCard, ex);
                    return StatusCode(500);
                }
            } 
        }


      
    }
}
