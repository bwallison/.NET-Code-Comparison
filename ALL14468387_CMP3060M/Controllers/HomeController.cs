using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace ALL14468387_CMP3060M.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
    }

    [HttpPost]
    public ActionResult Index(string name)
    {

        return "blah";
    }
}
