using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blog.Repoistory;
using Microsoft.AspNetCore.Mvc;

namespace BlogWeb
{
    [Route("home")]
    public class HomeController : Controller
    {
        private IArticleRepoistory _articleRepoistory;
        public HomeController(IArticleRepoistory articleRepoistory)
        {
            _articleRepoistory = articleRepoistory;
        }
        [HttpGet()]
        [Route("index")]
        public IActionResult Index()
        {
            _articleRepoistory.SelectById(1);
            return View();
        }
        [HttpGet()]
        [Route("square")]
        public IActionResult Square()
        {
            return View();
        }
    }
}