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
    public class ChangwatController : ControllerBase
    {
        private readonly IChangwatService changwatService;

        public ChangwatController(IChangwatService changwatService)
        {
            this.changwatService = changwatService;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return Ok(await changwatService.GetChangwat());
        }
    }
}
