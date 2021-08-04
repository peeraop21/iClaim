using Core.ViewModels;
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
    [Route("[controller]")]
    public class AccidentController : ControllerBase
    {
        private readonly IAccidentService accidentService;

        public AccidentController(IAccidentService accidentService) {
            this.accidentService = accidentService;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return Ok(await accidentService.GetAccident());
        }
    }
}
