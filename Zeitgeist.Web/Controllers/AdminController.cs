using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Zeitgeist.Web.Tools;

namespace Zeitgeist.Web.Controllers
{
       [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        //
        // GET: /Admin/

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult NavBar()
        {
            return View();
        }

        public ActionResult SideBar()
        {
            return View();
        }
    }
}
