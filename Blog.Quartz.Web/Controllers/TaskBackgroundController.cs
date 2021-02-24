using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Blog.Quartz.Web.Controllers
{
    [Route("task")]
    public class TaskBackgroundController: Controller
    {
        [Route("list")]
        public IActionResult Index()
        {
            return View();
        }
        [Route("list/page")]
        public IActionResult getListPage()
        {
            return View();
        }
    }
}
