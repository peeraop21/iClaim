
using Core.Models;
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

        public AccidentController(IAccidentService accidentService, IMasterService masterService) {
            this.accidentService = accidentService;
            this.masterService = masterService;
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
    }
}
