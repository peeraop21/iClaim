﻿
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Core.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccidentController : ControllerBase
    {
        private readonly IAccidentService accidentService;

        public AccidentController(IAccidentService accidentService) {
            this.accidentService = accidentService;
        }

        /*[HttpGet]
        public async Task<IActionResult> Get()
        {
            return Ok();
        }*/
        [Authorize]
        [HttpGet("{userToken}")]
        public async Task<IActionResult> Get(string userToken)
        {
            return Ok(await accidentService.GetAccident(userToken));
        }


        [HttpGet("Car/{accNo}/{channel}")]
        public async Task<IActionResult> GetAccidentCar(string accNo, string channel)
        {
            return Ok(await accidentService.GetAccidentCar(accNo.Replace("-","/"), channel));
        }

        [HttpGet("Victim/{accNo}/{ch}/{victimNo}")]
        public async Task<IActionResult> GetAccidentVictim(string accNo, string ch, int victimNo)
        {
            return Ok(await accidentService.GetAccidentVictim(accNo.Replace("-", "/"), ch, victimNo));
        }
    }
}
