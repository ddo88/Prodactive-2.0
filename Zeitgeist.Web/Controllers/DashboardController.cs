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
    [Authorize]
    public class DashboardController : Controller
    {
        private readonly RetoService _retoService;
        private readonly UserService _userService;
        private readonly WorkContext _workContextService;

        public DashboardController(RetoService retoService,
                                   UserService userService,
                                   WorkContext workContextService)
        {
            _retoService = retoService;
            _userService = userService;
            _workContextService = workContextService;
        }

        //
        // GET: /Dashboard/

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Challenges()
        {
            var userContext  = _workContextService.GetAuthenticatedUser();
            var model = new ChallengeModel();
            
            foreach (var reto in _retoService.GetChallengesWithUserRelated(userContext.User.Id))
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
            var userContext = _workContextService.GetAuthenticatedUser();
            var result = _retoService.GetStepsForLastXDaysChallenge(userContext.User.Id, 5);
            return Json(result);
        }
    }
}
