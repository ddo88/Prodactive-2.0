using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Zeitgeist.Web.Controllers
{
    public class ChatController : Controller
    {
        //
        // GET: /Chat/

        public ActionResult ChatView()
        {
            return View();
        }

        public JsonResult Messages()
        {

            return Json(new {}, JsonRequestBehavior.AllowGet);
        }

    }
}
