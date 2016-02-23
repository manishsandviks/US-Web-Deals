using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace deals.earlymoments.com.Areas.Max.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /Max/Home/
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult offers()
        {
            return View();
        }

    }
}