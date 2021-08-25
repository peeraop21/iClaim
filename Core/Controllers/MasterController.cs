using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Core.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MasterController : ControllerBase
    {
        private readonly IMasterService masterService;

        public MasterController(IMasterService masterService)
        {
            this.masterService = masterService;
        }

        
        [HttpGet("Bank")]
        public async Task<IActionResult> GetBank()
        {
            return Ok(await masterService.GetBank());
        }

        [HttpGet("Changwat")]
        public async Task<IActionResult> GetChangwat()
        {
            return Ok(await masterService.GetChangwat());
        }

        [HttpGet("Hospital")]
        public async Task<IActionResult> GetHospital()
        {
            return Ok(await masterService.GetHospital());
        }
    }
}
