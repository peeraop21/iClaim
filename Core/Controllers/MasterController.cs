using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
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
        private readonly ILogger<MasterController> logger;
        private readonly IConfiguration configuration;

        public MasterController(IMasterService masterService, ILogger<MasterController> logger, IConfiguration configuration)
        {
            this.masterService = masterService;
            this.logger = logger;
            this.configuration = configuration;
        }

        
        [HttpGet("Bank")]
        public async Task<IActionResult> GetBank()
        {
            try
            {
                return Ok(await masterService.GetBank());
            }
            catch (Exception ex)
            {
                string baseUrl = configuration["BaseUrl:Publish"];
                logger.LogError(baseUrl + ", API: GetBank, Exception: {0}", ex);
                return StatusCode(500);
            }
            
        }

        [HttpGet("Changwat")]
        public async Task<IActionResult> GetChangwat()
        {
            try
            {
                return Ok(await masterService.GetChangwatsAsync());
            }
            catch (Exception ex)
            {
                string baseUrl = configuration["BaseUrl:Publish"];
                logger.LogError(baseUrl + ", API: GetChangwat, Exception: {0}", ex);
                return StatusCode(500);
            }
            
        }

        [HttpGet("Wounded")]
        public async Task<IActionResult> GetWoundeds()
        {
            try
            {
                return Ok(await masterService.GetWoundeds());
            }
            catch (Exception ex)
            {
                string baseUrl = configuration["BaseUrl:Publish"];
                logger.LogError(baseUrl + ", API: GetWoundeds, Exception: {0}", ex);
                return StatusCode(500);
            }
            
        }

        [HttpGet("Prefix")]
        public async Task<IActionResult> GetPrefixes()
        {
            try
            {
                return Ok(await masterService.GetPrefixesAsync());
            }
            catch (Exception ex)
            {
                string baseUrl = configuration["BaseUrl:Publish"];
                logger.LogError(baseUrl + ", API: GetPrefixes, Exception: {0}", ex);
                return StatusCode(500);
            }
            
        }

        [HttpGet("Amphurs")]
        public async Task<IActionResult> GetAmphursByChangwatshortname(string changwatshortname)
        {
            try
            {
                return Ok(await masterService.GetAmphursByChangwatnameAsync(changwatshortname));
            }
            catch (Exception ex)
            {
                string baseUrl = configuration["BaseUrl:Publish"];
                logger.LogError(baseUrl + ", API: GetAmphursByChangwatshortname, Exception: {0}", ex);
                return StatusCode(500);
            }
        }

        [HttpGet("Tumbols")]

        public async Task<IActionResult> GetTumbolsByAmphurIdAsync(string changwatshortname, string amphurId)
        {
            try
            {
                return Ok(await masterService.GetTumbolsByAmphurIdAsync(changwatshortname, amphurId));
            }catch (Exception ex)
            {
                string baseUrl = configuration["BaseUrl:Publish"];
                logger.LogError(baseUrl + ", API: GetTumbolsByAmphurIdAsync, Exception: {0}", ex);
                return StatusCode(500);
            }
            
        }


    }
}
