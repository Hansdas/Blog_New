using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlogGateway.Controllers
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
        [Route("test")]
        [HttpPost]
        public string TestOcelot()
        {
            return "ok";
        }
    }
}
