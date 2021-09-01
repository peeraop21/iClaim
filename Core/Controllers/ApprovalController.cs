using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Services;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Core.Controllers
{
    
    [Route("api/[controller]")]
    [ApiController]
    public class ApprovalController : ControllerBase
    {
        private readonly IApprovalService approvalService;
        public ApprovalController(IApprovalService approvalService)
        {
            this.approvalService = approvalService;
        }
        [HttpGet("{accNo}")]
        public async Task<IActionResult> GetApproval(string accNo)
        {
            return Ok(await approvalService.GetApproval(accNo.Replace("-", "/")));
        }

        // POST api/<ApprovalController>
        [HttpPost]
        public IActionResult Post([FromBody]object model)
        {
            return Ok(new { status = "" });
        }

    }
}
