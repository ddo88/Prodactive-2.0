using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Zeitgeist.Web.Models.Dashboard;
using Zeitgeist.Web.Services;
using Zeitgeist.Web.Tools;

namespace Zeitgeist.Web.Controllers
{
    public class DashboardController : Controller
    {
        private readonly RetoService _retoService;
        private readonly UserService _userService;
        private readonly Security _securityService;

        public DashboardController(RetoService retoService,
                                   UserService userService,
                                   Security securityService)
        {
            _retoService = retoService;
            _userService = userService;
            _securityService = securityService;
        }

        //
        // GET: /Dashboard/

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Challenges()
        {
            var user  = _securityService.GetAuthenticatedUser();
            var model = new ChallengeModel();
            
            foreach (var reto in _retoService.GetChallengesWithUserRelated(user.Id))
            {
                model.DetailChallenges.Add(new DetailChallenge { Name = reto.Name, Goal = reto.Meta, MyProgress = _retoService.GetStepsForChallenge(reto.Id) });
            }
            
            return View(model);
        }

        public ActionResult Resume()
        {
            return View();
        }

        public ActionResult Medals()
        {
            return View();
        }

        [HttpPost]
        public JsonResult GetResume()
        {
            var user = _securityService.GetAuthenticatedUser();
            var result = _retoService.GetStepsForLastXDaysChallenge(user.Id, 5);
            return Json(result);
        }
    }
}
