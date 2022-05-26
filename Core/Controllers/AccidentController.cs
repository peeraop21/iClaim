
using AutoMapper;
using Core.Models;
using DataAccess.EFCore.RvpOfficeModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services;
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

        public AccidentController(IAccidentService accidentService, IMasterService masterService, IMapper _mapper, IHttpContextAccessor httpContextAccessor) {
            this.accidentService = accidentService;
            this.masterService = masterService;
            this._mapper = _mapper;
            this.httpContextAccessor = httpContextAccessor;
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> GetAccident([FromBody] ReqData req)
        {
            return Ok(await accidentService.GetAccidentByIdLine(req.UserIdLine));
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
                Cars = await accidentService.GetEpoliciesByIdCardAsync(req.UserIdCard)
            });
        }

        [HttpPost("InsertAccident")]
        public async Task<IActionResult> PostAccident([FromBody] ReqPostAccident req)
        {
            var ip = httpContextAccessor.HttpContext.Connection.RemoteIpAddress.ToString();
            var hosAccident = _mapper.Map<HosAccident>(req.AccidentInput);
            var hosCarAccident = _mapper.Map<HosCarAccident>(req.AccidentCarInput);
            var hosVicTimAccident = _mapper.Map<HosVicTimAccident>(req.AccidentVictimInput);

            var result = await accidentService.AddAsync(hosAccident, hosCarAccident, hosVicTimAccident, ip);
            return Ok();
        }
    }
}
