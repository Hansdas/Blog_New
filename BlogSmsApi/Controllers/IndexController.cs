using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlogSmsApi.Controllers
{
    [Route("api")]
    [ApiController]
    public class IndexController : ControllerBase
    {
        [Route("start")]
        [HttpGet]
        public string Start()
        {
            return "api service start";
        }
        [HttpGet("health")]
        public IActionResult Heathl()
        {
            return Ok();
        }
    }
}
