using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Blog.Quartz.Web.Controllers
{
    public class HomeController: Controller
    {
        //[Route("/")]
        //public IActionResult getIndex()
        //{
        //    return View("~/pages/index.cshtml");
        //}
        [Route("home/index")]
        [HttpGet]
        public IActionResult Index()
        {
            return View("~/pages/index.cshtml");
        }
    }
}
