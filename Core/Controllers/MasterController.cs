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
    [Route("api/[controller]")]
    [ApiController]
    public class MasterController : ControllerBase
    {
        private readonly IMasterService masterService;
        private readonly IWarmupService warmupService;

        public MasterController(IMasterService masterService, IWarmupService warmupService)
        {
            this.masterService = masterService;
            this.warmupService = warmupService;
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

        [HttpGet("Wounded")]
        public async Task<IActionResult> GetWoundeds()
        {
            return Ok(await masterService.GetWoundeds());
        }

        [HttpGet("Prefix")]
        public async Task<IActionResult> GetPrefixes()
        {
            return Ok(await masterService.GetPrefixesAsync());
        }

        [HttpGet("Warmup")]
        public async Task<IActionResult> WarmupApi()
        {
            return Ok(await warmupService.GetDataForWarmup());
        }


    }
}
