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
            return Ok(await masterService.GetChangwatsAsync());
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

        [HttpGet("Amphurs")]
        public async Task<IActionResult> GetAmphursByChangwatshortname(string changwatshortname)
        {
            return Ok(await masterService.GetAmphursByChangwatnameAsync(changwatshortname));
        }

        [HttpGet("Tumbols")]

        public async Task<IActionResult> GetTumbolsByAmphurIdAsync(string changwatshortname, string amphurId)
        {
            return Ok(await masterService.GetTumbolsByAmphurIdAsync(changwatshortname, amphurId));
        }


    }
}
