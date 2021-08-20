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
    public class BankNamesController : ControllerBase
    {
        private readonly IBankNamesService bankNamesService;

        public BankNamesController(IBankNamesService bankNamesService)
        {
            this.bankNamesService = bankNamesService;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return Ok(await bankNamesService.GetBank());
        }
    }
}
