
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

        public AccidentController(IAccidentService accidentService) {
            this.accidentService = accidentService;
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
    }
}
