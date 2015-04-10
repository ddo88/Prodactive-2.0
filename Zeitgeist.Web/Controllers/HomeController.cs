using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Zeitgeist.Core.Data.Interfaces;
using Zeitgeist.Web.Domain;
using Zeitgeist.Web.Models.Page;
using Zeitgeist.Web.Services;
using Zeitgeist.Web.Tools;

namespace Zeitgeist.Web.Controllers
{
    [Authorize]
    
    public class HomeController : Controller
    {
        private readonly WorkContext _workContext;
        private readonly Media _media;
        private readonly RetoService _retoService;
        private readonly IRepository<Plan> _planRepository;
        private readonly IRepository<Liga> _ligaRepository;
        private readonly IRepository<Deporte> _deporteRepository;
        private readonly IRepository<Equipo> _equipoRepository;
        private readonly IRepository<Division> _divisionRepository;
        private readonly IRepository<EquipoDivisionMapping> _equipoDivisionRepository;
        private readonly IRepository<Reto> _retoRepository;
        private readonly IRepository<TipoReto> _tipoRetoRepository;
        private readonly IRepository<Premio> _premioRepository;
        private readonly IRepository<RetoEquiposMapping> _retoEquiposRepository;
        private readonly IRepository<Setting> _settingRepository;

        public HomeController(WorkContext workContext, Media media, RetoService retoService,
            IRepository<Plan> planRepository, IRepository<Liga> ligaRepository,
            IRepository<Deporte> deporteRepository,IRepository<Equipo> equipoRepository ,
            IRepository<Division> divisionRepository, IRepository<EquipoDivisionMapping> equipoDivisionRepository,
            IRepository<Reto> retoRepository,IRepository<TipoReto> tipoRetoRepository,
            IRepository<Premio> premioRepository, IRepository<RetoEquiposMapping> retoEquiposRepository,IRepository<Setting> settingRepository )
        {
            _workContext = workContext;
            _media = media;
            _retoService = retoService;
            _planRepository = planRepository;
            _ligaRepository = ligaRepository;
            _deporteRepository = deporteRepository;
            _equipoRepository = equipoRepository;
            _divisionRepository = divisionRepository;
            _equipoDivisionRepository = equipoDivisionRepository;
            _retoRepository = retoRepository;
            _tipoRetoRepository = tipoRetoRepository;
            _premioRepository = premioRepository;
            _retoEquiposRepository = retoEquiposRepository;
            _settingRepository = settingRepository;
        }

        //
        // GET: /Home/
        public ActionResult Index()
        {

            
            /* only for test*/
            //Roles.CreateRole(Security.AppRoles.Admin);
            //Roles.CreateRole(Security.AppRoles.Athlete);
            //Roles.CreateRole(Security.AppRoles.Coach);

            //var user = _security.GetAuthenticatedUser();
            //Roles.AddUserToRole(user.User.UserName, Security.AppRoles.Admin);
            //Roles.AddUserToRole(user.User.UserName, Security.AppRoles.Athlete);
            //Roles.AddUserToRole(user.User.UserName, Security.AppRoles.Coach);

            //createData();
            //var user = _security.GetAuthenticatedUser();
            return RedirectToAction("Index", "Dashboard");
            //return View();
        }

        [ChildActionOnly]
        public ActionResult LeftMenu()
        {
            MenuModel menu = new MenuModel();

            var m = new List<MenuItem>()
            {
                new MenuItem() {Href = "A", Name = "A"},
                new MenuItem() {Href = "B", Name = "B"}
            };
            menu.MenuItems = new List<MenuItem>
            {
                new MenuItem(){Href = "/Home/Dashboard",Name = "Inicio",IconClass = MenuIcon.Dashboard ,Active=true},
                new MenuItem(){Name = "Retos",IconClass = MenuIcon.Challenge , SubMenuItems = GetRetos().ToSubMenuItems()},
                new MenuItem(){Name = "Estadisticas",IconClass = MenuIcon.Statistics},
                new MenuItem(){Name = "Logros",IconClass = MenuIcon.Achievements},
                new MenuItem(){Name = "Tips",IconClass = MenuIcon.Tips, SubMenuItems = m},
                new MenuItem(){Name = "Calendario",IconClass = MenuIcon.Calendar},
                new MenuItem(){Name = "Galeria",IconClass = MenuIcon.Galery},
                new MenuItem(){Name = "Acerca De",IconClass = MenuIcon.About},
                new MenuItem(){Name = "FAQ",IconClass = MenuIcon.Questions}
            };

            
            return View(menu);
        }

        
        [ChildActionOnly]
        public ActionResult NavBar()
        {
            var userContext = _workContext.GetAuthenticatedUser();
            NavbarModel model = new NavbarModel();

            PrepareLeagueModel  (model, userContext);
            PrepareUserDataModel(model, userContext.User);
            
            return View(model);
        }

        public ActionResult Breadcrumbs()
        {
            return View();
        }

        [NonAction]
        private List<Reto> GetRetos()
        {
            var userContext = _workContext.GetAuthenticatedUser();
            return _retoService.GetChallengesWithIdLeague(userContext.IdLeague);
            //return _retoService.GetChallengesWithUserRelated(userContext.User.Id);
        }

        [NonAction]
        private void PrepareLeagueModel(NavbarModel model, UserContext userContext)
        {

            if (userContext.User.UserSettings == null || userContext.User.UserSettings.Count == 0)
            {
                try
                {
                    var ligas = userContext.User.Ligas; 
                    if (ligas != null && ligas.Count>0)
                        _workContext.SaveUserSetting(SettingBase.League, ligas.FirstOrDefault().Id.ToString());
                }
                catch (Exception ex)
                {
                    //REV
                }
                
            }

            foreach (var liga in userContext.User.Ligas)
            {
                model.Leagues.Leagues.Add(new League(){Name = liga.Name, IdLeague=liga.Id,Selected=(userContext.IdLeague==liga.Id)});    
            }
        }
        
        [NonAction]
        private void PrepareUserDataModel(NavbarModel model, User user)
        {

            var roles=Roles.GetRolesForUser(user.UserName);
            model.User.Name = user.Persona.GetFullName();
            string r = _media.GetUserAvatarUrl(user.UserName);
            if (!String.IsNullOrEmpty(r))
                model.User.AvatarUrl = Url.Content(r);
            else
                model.User.AvatarUrl = r;

            if (roles.Contains(WorkContext.AppRoles.Admin))
            {
                model.User.AccessToAdmin = true;
            }
        }

        private void createData()
        {
            
            Plan p= new Plan();
            p.Name = "Freemium";
            _planRepository.Insert(p);

            Plan p2= new Plan();
            p2.Name = "Standard";
            _planRepository.Insert(p2);

            Liga liga= new Liga();
            liga.Name = "liga Daniel";
            liga.User = _workContext.GetAuthenticatedUser().User;
            liga.Plan = p;
            _ligaRepository.Insert(liga);

            Deporte deporte= new Deporte();
            deporte.Name         = "Caminar";
            deporte.TipoConteo   = TipoConteo.Pasos;
            deporte.FactorConteo = 1;
            _deporteRepository.Insert(deporte);

            
            Equipo equipo= new Equipo();
            equipo.Name = "A";
            
            _equipoRepository.Insert(equipo);

            Equipo equipob= new Equipo();
            equipob.Name = "B";
            _equipoRepository.Insert(equipob);

            Division division= new Division();
            division.Name = "Individual";
            _divisionRepository.Insert(division);
             
           



            EquipoDivisionMapping eq = new EquipoDivisionMapping();
            eq.DivisionId = 1;
            eq.EquipoId = 1;
            _equipoDivisionRepository.Insert(eq);

            EquipoDivisionMapping eq1 = new EquipoDivisionMapping();
            eq1.DivisionId = 1;
            eq1.EquipoId = 2;
            _equipoDivisionRepository.Insert(eq1);

            TipoReto tr = new TipoReto();
            tr.Name = "Todos contra todos";
            _tipoRetoRepository.Insert(tr);
            TipoReto tr2 = new TipoReto();
            tr2.Name = "primero en llegar";
            _tipoRetoRepository.Insert(tr2);

            Premio pp = new Premio();
            pp.Name = "Regalo X";
            _premioRepository.Insert(pp);


            Reto reto = new Reto();
            reto.Coach = _workContext.GetAuthenticatedUser().User;
            reto.DivisionId = 1;
            reto.FechaInicio = DateTime.Now;
            reto.FechaFin = DateTime.Now.AddMonths(5);
            reto.IsActivo = true;
            reto.LigaId = 1;
            reto.Meta = 20000;
            reto.Owner = _workContext.GetAuthenticatedUser().User;
            reto.PremioId = p.Id;
            reto.TipoRetoId = 1;
            _retoRepository.Insert(reto);

            RetoEquiposMapping re = new RetoEquiposMapping();
            re.EquipoId = 1;
            re.RetoId = 1;
            _retoEquiposRepository.Insert(re);
            RetoEquiposMapping re2 = new RetoEquiposMapping();
            re2.EquipoId = 2;
            re2.RetoId = 1;
            _retoEquiposRepository.Insert(re2);
        }

        public ActionResult ChangeLeague(int id)
        {
            _workContext.GetAuthenticatedUser().IdLeague = id;
            return RedirectToAction("Index");
        }

    }
}
