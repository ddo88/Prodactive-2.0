using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.Security;
using Newtonsoft.Json;
using Zeitgeist.Core.Data.Interfaces;
using Zeitgeist.Web.Domain;
using Zeitgeist.Web.Models;
using Zeitgeist.Web.Tools;

namespace Zeitgeist.Web.Controllers
{
    [Authorize]
    //[InitializeSimpleMembership]
    public class AccountController : Controller
    {
        private readonly IRepository<User> _useRepository;
        private readonly WorkContext _workContext;
        private readonly InMemoryCache _memoryCache;

        public AccountController(IRepository<User> useRepository,
                                 WorkContext          workContext,
                                 InMemoryCache     memoryCache)
        {
            _useRepository = useRepository;
            _workContext = workContext;
            _memoryCache = memoryCache;
        }

        //
        // GET: /Account/Login
        //private static readonly log4net.ILog log = log4net.LogManager.GetLogger(typeof(AccountController));
        //private Manager manager = Manager.Instance;
        [AllowAnonymous]
        public ActionResult Index()
        {
            //Roles.CreateRole(Rol.Root.ToString());
            //Roles.CreateRole(Rol.Atleta.ToString());
            return View();
        }

        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }
        [AllowAnonymous]
        public ActionResult ForgotPassword()
        {
            return View();
        }

        //
        // POST: /Account/Login

        //    [HttpPost]
        //    [AllowAnonymous]
        //    [ValidateJsonAntiForgeryToken]
        //    //[ValidateAntiForgeryToken]
        //    public JsonResult Login(string dataSave, string returnUrl)
        //    {
        //        LoginModel model = JsonConvert.DeserializeObject<LoginModel>(dataSave);
        //        returnUrl        = model.ReturnUrl;
        //        if (String.IsNullOrEmpty(returnUrl))
        //            returnUrl = this.Url.Action("Index", "Home", null, this.Request.Url.Scheme);

        //        if (ModelState.IsValid && Membership.ValidateUser(model.UserName, model.Password))
        //        {
        //            FormsAuthentication.SetAuthCookie(model.UserName, model.RememberMe);

        //            /*
        //             * //create the authentication ticket
        //var authTicket = new FormsAuthenticationTicket(
        //  1,
        //  userId,  //user id
        //  DateTime.Now,
        //  DateTime.Now.AddMinutes(2000),  // expiry in minutes
        //  rememberMe,  //true to remember if it is checked
        //  "", //roles 
        //  "/"
        //);

        ////encrypt the ticket and add it to a cookie
        //HttpCookie cookie = new HttpCookie(FormsAuthentication.FormsCookieName,   FormsAuthentication.Encrypt(authTicket));
        //Response.Cookies.Add(cookie);
        //             */
        //            Session.Add("UserId", model.UserName);

        //            return Json(new { success = true, redirect = returnUrl });
        //        }
        //        // Si llegamos a este punto, es que se ha producido un error y volvemos a mostrar el formulario
        //        ModelState.AddModelError("", "El nombre de usuario o la contraseña especificados son incorrectos.");
        //        return Json(new { errors = GetErrorsFromModelState() });


        //    }
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginModel model, string returnUrl)
        {
            returnUrl = model.ReturnUrl;
            if (String.IsNullOrEmpty(returnUrl))
                returnUrl = this.Url.Action("Index", "Home", null, this.Request.Url.Scheme);
            if (ModelState.IsValid && Membership.ValidateUser(model.UserName, model.Password))
            {
                FormsAuthentication.SetAuthCookie(model.UserName, model.RememberMe);
                Session.Add("UserId", model.UserName);
                return Json(new { success = true, redirect = returnUrl });
            }
            //// Si llegamos a este punto, es que se ha producido un error y volvemos a mostrar el formulario
            return
                Json(
                    new
                    {
                        success = false,
                        errorField = "UserName",
                        message = "El nombre de usuario o la contraseña especificados son incorrectos."
                    });
        }

        public ActionResult LogOff()
        {
            //WebSecurity.Logout();
            //return RedirectToAction("Index", "Home");
            var userContext = _workContext.GetAuthenticatedUser();
            _memoryCache.DeleteCache(userContext.User.UserName);
            FormsAuthentication.SignOut();
            Session.Abandon();

            return RedirectToAction("Index", "Home");
        }

        [AllowAnonymous]
        public ActionResult Register(string returnUrl)
        {
            //Roles.CreateRole("Root");
            //Roles.CreateRole("Atleta");
            //ViewBag.ReturnUrl = returnUrl;

            RegisterModel model = new RegisterModel();
            List<SelectListItem> lst = new List<SelectListItem>();
            GetQuestions().ForEach(a =>
            {
                lst.Add(new SelectListItem() { Text = a, Value = a });
            });
            model.AvailableQuestions = lst;
            model.AvailableSex = GetSexos();

            return View(model);
        }

        private static List<SelectListItem> GetSexos()
        {
            List<SelectListItem> lst2 = new List<SelectListItem>();
            lst2.Add(new SelectListItem() { Text = "Masculino", Value = ((int)Sexo.Masculino).ToString() });
            lst2.Add(new SelectListItem() { Text = "Femenino", Value = ((int)Sexo.Femenino).ToString() });
            return lst2;
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public JsonResult Register(RegisterModel model, string returnUrl)
        {
            //if (dataSave == null)
            //{
            //    return Json(new {success = false});
            //    //return Json(new { success = true, redirect = fullUrl });     
            //}

            //var serializer = new JsonSerializerSettings()
            //{
            //    MissingMemberHandling = MissingMemberHandling.Ignore,
            //    CheckAdditionalContent = false
            //};
            //RegisterModel model = JsonConvert.DeserializeObject<RegisterModel>(dataSave, serializer);

            if (ModelState.IsValid)
            {
                // Intento de registrar al usuario
                //try
                //{
                MembershipCreateStatus status;
                Membership.CreateUser(model.UserName, model.Password, model.Email, model.PasswordQuestion,
                    model.PasswordAnswers, true, out status);
                //    //WebSecurity.CreateUserAndAccount(model.UserName, model.Password);
                //    //WebSecurity.Login(model.UserName, model.Password);

                if (status == MembershipCreateStatus.Success)
                {

                    Persona p = new Persona
                    {
                        Apellido = model.Apellido,
                        Estatura = model.Estatura,
                        FechaNacimiento = model.FechaNacimiento,
                        Identificacion = model.Identificacion,
                        Nombre = model.Nombre,
                        Peso = model.Peso,
                        SexoId = model.SexoId
                    };

                    User user = new User()
                    {
                        Email = model.Email,
                        UserName = model.UserName,
                        Persona = p
                    };

                    _useRepository.Insert(user);
                    _memoryCache.DeleteCache(user.UserName);
                    //Roles.AddUserToRole(model.UserName, "Root");
                    FormsAuthentication.SetAuthCookie(model.UserName, false /* createPersistentCookie */);
                }
                returnUrl = this.Url.Action("Index", "Home", null, this.Request.Url.Scheme);
            }
            else
            {
                returnUrl = this.Url.Action("Index", "Home", null, this.Request.Url.Scheme);
            }


            return Json(new { redirect = (String.IsNullOrEmpty(returnUrl) ? "/Home/Index" : returnUrl) }); //errors = GetErrorsFromModelState()

            // Si llegamos a este punto, es que se ha producido un error y volvemos a mostrar el formulario
            //return View(model);
        }

        private List<string> GetQuestions()
        {
            List<string> elm = new List<string>()
            {
                "Nombre de tu primera Mascota",
                "Marca de tu primer Vehiculo",
                "Nombre de tu Padre",
                "Nombre de tu Madre",
                "Mes de Nacimiento de tu hijo/hija"
            };
            return elm;
        }


//        private IEnumerable<string> GetErrorsFromModelState()
//        {
//            return ModelState.SelectMany(x => x.Value.Errors.Select(error => error.ErrorMessage));
//        }

//        [AllowAnonymous]
//        public ActionResult ResetAccount()
//        {
//            var list =
//                GetQuestions().Select(x => new SelectListItem() { Value = x.ToString(), Text = x.ToString() }).ToList();
//            SelectList sl = new SelectList(list, "Value", "Text");
//            ViewBag.Questions = sl;
//            return View();
//        }

//        [HttpPost]
//        [AllowAnonymous]
//        public ActionResult ResetAccount(FormCollection form)
//        {
//            try
//            {

//                string user = form["usuario"];
//                string pass = form["password"];
//                string email = form["email"];
//                string question = form["pregunta"];
//                string answer = form["answer"];

//                Membership.DeleteUser(user);
//                MembershipCreateStatus status;
//                Membership.CreateUser(user, pass, email, question, answer, true, out status);
//                if (status == MembershipCreateStatus.Success)
//                {
//                    string htmlBody = "<h5>Usuario creado correctamente en prodactive</h5>";
//                    manager.SendMail(email, "Usuario creado en Prodactive", htmlBody);
//                    return RedirectToAction("Login");

//                }
//            }
//            catch (Exception ex)
//            {
//                log.Error("Reset Password", ex);
//            }

//            var list =
//                GetQuestions().Select(x => new SelectListItem() { Value = x.ToString(), Text = x.ToString() }).ToList();
//            SelectList sl = new SelectList(list, "Value", "Text");
//            ViewBag.Questions = sl;
//            ViewBag.Error = true;
//            ViewBag.Mensaje = "Verifique los datos ingresados";
//            return View();
//        }





//        [AllowAnonymous]
//        public ActionResult ResetPassword(string id)
//        {
//            var datos = Membership.FindUsersByEmail(id);
//            MembershipUser m = datos[Membership.GetUserNameByEmail(id)];

//            return View();
//        }

//        [AllowAnonymous]
//        public ActionResult RecoverPassword(string id)
//        {
//            if (String.IsNullOrEmpty(id))
//            {
//            }
//            var datos = Membership.FindUsersByEmail(id);

//            ViewBag.Mail = id;
//            ViewBag.Question = datos[Membership.GetUserNameByEmail(id)].PasswordQuestion;
//            return View();

//        }


//        [HttpPost]
//        [AllowAnonymous]
//        public ActionResult RecoverPassword(FormCollection form, string id)
//        {

//            var datos = Membership.FindUsersByEmail(form["id"]);
//            MembershipUser user = datos[Membership.GetUserNameByEmail(id)];
//            string pass = user.GetPassword(form["answer"]);



//            string body = "<h3>Contraseña Prodactive:</h3> " + pass +
//                          "</br> Recuerda cambiar tu contraseña desde el panel de administración.";

//            manager.SendMail(id, "Prodactive: Recuperacion de contraseña", body);

//            return RedirectToAction("RecoverSend");
//        }

//        [HttpPost]
//        [AllowAnonymous]
//        public ActionResult SendRecoverPassword(FormCollection form)
//        {

//            if (!String.IsNullOrEmpty(form["email"]))
//            {


//                string url = "http://prodactive.co/Account/RecoverPassword/" + form["email"];
//                string message =
//                    @"<a href='" + url + @"' target='_blank'>
//                                    Generar nueva contraseña
//                                </a>";


//                manager.SendMail(form["email"], "Recuperacion Contraseña Prodactive", message);
//                return RedirectToAction("RecoverSend");
//            }
//            else
//            {
//                //Rev: revisar esta parte
//                return RedirectToAction("Error");
//            }

//        }


//        //mensaje de Recuperación de contraseña enviada
//        [AllowAnonymous]
//        public ActionResult RecoverSend()
//        {
//            return View();
//        }

//        //
//        // POST: /Account/LogOff

            //[HttpPost]
            //[ValidateAntiForgeryToken]
            

//        //
//        // GET: /Account/Register

        

//        //
//        // POST: /Account/Register

        
//        [HttpPost]
//        [AllowAnonymous]
//        public JsonResult Questions()
//        {
//            var elm = GetQuestions();

//            return Json(elm, JsonRequestBehavior.AllowGet);
//        }

//        private static List<string> GetQuestions()
//        {
//            List<string> elm = new List<string>()
//            {
//                "Nombre de tu primera Mascota",
//                "Marca de tu primer Vehiculo",
//                "Nombre de tu Padre",
//                "Nombre de tu Madre",
//                "Mes de Nacimiento de tu hijo/hija"
//            };
//            return elm;
//        }


//        //
//        // GET: /Account/Manage

//        public ActionResult Manage(ManageMessageId? message)
//        {
//            ViewBag.StatusMessage =
//                message == ManageMessageId.ChangePasswordSuccess ? "La contraseña se ha cambiado."
//                    : message == ManageMessageId.SetPasswordSuccess ? "Su contraseña se ha establecido."
//                        : message == ManageMessageId.RemoveLoginSuccess ? "El inicio de sesión externo se ha quitado."
//                            : "";
//            ViewBag.HasLocalPassword = false;// OAuthWebSecurity.HasLocalAccount(WebSecurity.GetUserId(User.Identity.Name));
//            ViewBag.ReturnUrl = Url.Action("Manage");

//            return View();
//        }

//        //
//        // POST: /Account/Manage

//        //[HttpPost]
//        //[ValidateAntiForgeryToken]
//        //public ActionResult Manage(LocalPasswordModel model)
//        //{
//        //    bool hasLocalAccount = false;//OAuthWebSecurity.HasLocalAccount(WebSecurity.GetUserId(User.Identity.Name));
//        //    ViewBag.HasLocalPassword = hasLocalAccount;
//        //    ViewBag.ReturnUrl = Url.Action("Manage");
//        //    if (hasLocalAccount)
//        //    {
//        //        if (ModelState.IsValid)
//        //        {
//        //            // ChangePassword iniciará una excepción en lugar de devolver false en determinados escenarios de error.
//        //            bool changePasswordSucceeded;
//        //            try
//        //            {
//        //                changePasswordSucceeded = WebSecurity.ChangePassword(User.Identity.Name, model.OldPassword, model.NewPassword);
//        //            }
//        //            catch (Exception)
//        //            {
//        //                changePasswordSucceeded = false;
//        //            }

//        //            if (changePasswordSucceeded)
//        //            {
//        //                return RedirectToAction("Manage", new { Message = ManageMessageId.ChangePasswordSuccess });
//        //            }
//        //            else
//        //            {
//        //                ModelState.AddModelError("", "La contraseña actual es incorrecta o la nueva contraseña no es válida.");
//        //            }
//        //        }
//        //    }
//        //    else
//        //    {
//        //        // El usuario no dispone de contraseña local, por lo que debe quitar todos los errores de validación generados por un
//        //        // campo OldPassword
//        //        ModelState state = ModelState["OldPassword"];
//        //        if (state != null)
//        //        {
//        //            state.Errors.Clear();
//        //        }

//        //        if (ModelState.IsValid)
//        //        {
//        //            try
//        //            {
//        //                WebSecurity.CreateAccount(User.Identity.Name, model.NewPassword);
//        //                return RedirectToAction("Manage", new { Message = ManageMessageId.SetPasswordSuccess });
//        //            }
//        //            catch (Exception)
//        //            {
//        //                ModelState.AddModelError("", String.Format("No se puede crear una cuenta local. Es posible que ya exista una cuenta con el nombre \"{0}\".", User.Identity.Name));
//        //            }
//        //        }
//        //    }

//        //    // Si llegamos a este punto, es que se ha producido un error y volvemos a mostrar el formulario
//        //    return View(model);
//        //}

//        //
//        // POST: /Account/ExternalLogin

//        //[HttpPost]
//        //[AllowAnonymous]
//        //[ValidateAntiForgeryToken]
//        //public ActionResult ExternalLogin(string provider, string returnUrl)
//        //{
//        //    return new ExternalLoginResult(provider, Url.Action("ExternalLoginCallback", new { ReturnUrl = returnUrl }));
//        //}

//        //
//        // GET: /Account/ExternalLoginCallback

//        //[AllowAnonymous]
//        //public ActionResult ExternalLoginCallback(string returnUrl)
//        //{
//        //    AuthenticationResult result = OAuthWebSecurity.VerifyAuthentication(Url.Action("ExternalLoginCallback", new { ReturnUrl = returnUrl }));
//        //    if (!result.IsSuccessful)
//        //    {
//        //        return RedirectToAction("ExternalLoginFailure");
//        //    }

//        //    if (OAuthWebSecurity.Login(result.Provider, result.ProviderUserId, createPersistentCookie: false))
//        //    {
//        //        return RedirectToLocal(returnUrl);
//        //    }

//        //    if (User.Identity.IsAuthenticated)
//        //    {
//        //        // Si el usuario actual ha iniciado sesión, agregue la cuenta nueva
//        //        OAuthWebSecurity.CreateOrUpdateAccount(result.Provider, result.ProviderUserId, User.Identity.Name);
//        //        return RedirectToLocal(returnUrl);
//        //    }
//        //    else
//        //    {
//        //        // El usuario es nuevo, solicitar nombres de pertenencia deseados
//        //        string loginData = OAuthWebSecurity.SerializeProviderUserId(result.Provider, result.ProviderUserId);
//        //        ViewBag.ProviderDisplayName = OAuthWebSecurity.GetOAuthClientData(result.Provider).DisplayName;
//        //        ViewBag.ReturnUrl = returnUrl;
//        //        return View("ExternalLoginConfirmation", new RegisterExternalLoginModel { UserName = result.UserName, ExternalLoginData = loginData });
//        //    }
//        //}

//        //
//        // POST: /Account/ExternalLoginConfirmation

//        //[HttpPost]
//        //[AllowAnonymous]
//        //[ValidateAntiForgeryToken]
//        //public ActionResult ExternalLoginConfirmation(RegisterExternalLoginModel model, string returnUrl)
//        //{
//        //    string provider = null;
//        //    string providerUserId = null;

//        //    if (User.Identity.IsAuthenticated || !OAuthWebSecurity.TryDeserializeProviderUserId(model.ExternalLoginData, out provider, out providerUserId))
//        //    {
//        //        return RedirectToAction("Manage");
//        //    }

//        //    if (ModelState.IsValid)
//        //    {
//        //        // Insertar un nuevo usuario en la base de datos
//        //        using (UsersContext db = new UsersContext())
//        //        {
//        //            UserProfile user = db.UserProfiles.FirstOrDefault(u => u.UserName.ToLower() == model.UserName.ToLower());
//        //            // Comprobar si el usuario ya existe
//        //            if (user == null)
//        //            {
//        //                // Insertar el nombre en la tabla de perfiles
//        //                db.UserProfiles.Add(new UserProfile { UserName = model.UserName });
//        //                db.SaveChanges();

//        //                OAuthWebSecurity.CreateOrUpdateAccount(provider, providerUserId, model.UserName);
//        //                OAuthWebSecurity.Login(provider, providerUserId, createPersistentCookie: false);

//        //                return RedirectToLocal(returnUrl);
//        //            }
//        //            else
//        //            {
//        //                ModelState.AddModelError("UserName", "El nombre de usuario ya existe. Escriba un nombre de usuario diferente.");
//        //            }
//        //        }
//        //    }

//        //    ViewBag.ProviderDisplayName = OAuthWebSecurity.GetOAuthClientData(provider).DisplayName;
//        //    ViewBag.ReturnUrl = returnUrl;
//        //    return View(model);
//        //}

//        //
//        // GET: /Account/ExternalLoginFailure
//        /*
//        [AllowAnonymous]
//        public ActionResult ExternalLoginFailure()
//        {
//            return View();
//        }

//        [AllowAnonymous]
//        [ChildActionOnly]
//        public ActionResult ExternalLoginsList(string returnUrl)
//        {
//            ViewBag.ReturnUrl = returnUrl;
//            return PartialView("_ExternalLoginsListPartial", OAuthWebSecurity.RegisteredClientData);
//        }

//        [ChildActionOnly]
//        public ActionResult RemoveExternalLogins()
//        {
//            ICollection<OAuthAccount> accounts = new Collection<OAuthAccount>();// OAuthWebSecurity.GetAccountsFromUserName(User.Identity.Name);
//            List<ExternalLogin> externalLogins = new List<ExternalLogin>();
//            foreach (OAuthAccount account in accounts)
//            {
//                AuthenticationClientData clientData = OAuthWebSecurity.GetOAuthClientData(account.Provider);

//                externalLogins.Add(new ExternalLogin
//                {
//                    Provider = account.Provider,
//                    ProviderDisplayName = clientData.DisplayName,
//                    ProviderUserId = account.ProviderUserId,
//                });
//            }

//            ViewBag.ShowRemoveButton = externalLogins.Count > 1 || false;//OAuthWebSecurity.HasLocalAccount(WebSecurity.GetUserId(User.Identity.Name));
//            return PartialView("_RemoveExternalLoginsPartial", externalLogins);
//        }*/

//        #region Aplicaciones auxiliares
//        private ActionResult RedirectToLocal(string returnUrl)
//        {
//            if (Url.IsLocalUrl(returnUrl))
//            {
//                return Redirect(returnUrl);
//            }
//            else
//            {
//                return RedirectToAction("Index", "Home");
//            }
//        }

//        public enum ManageMessageId
//        {
//            ChangePasswordSuccess,
//            SetPasswordSuccess,
//            RemoveLoginSuccess,
//        }

//        //internal class ExternalLoginResult : ActionResult
//        //{
//        //    public ExternalLoginResult(string provider, string returnUrl)
//        //    {
//        //        Provider = provider;
//        //        ReturnUrl = returnUrl;
//        //    }

//        //    public string Provider { get; private set; }
//        //    public string ReturnUrl { get; private set; }

//        //    public override void ExecuteResult(ControllerContext context)
//        //    {
//        //        OAuthWebSecurity.RequestAuthentication(Provider, ReturnUrl);
//        //    }
//        //}

//        private static string ErrorCodeToString(MembershipCreateStatus createStatus)
//        {
//            // Vaya a http://go.microsoft.com/fwlink/?LinkID=177550 para
//            // obtener una lista completa de códigos de estado.
//            switch (createStatus)
//            {
//                case MembershipCreateStatus.DuplicateUserName:
//                    return "El nombre de usuario ya existe. Escriba un nombre de usuario diferente.";

//                case MembershipCreateStatus.DuplicateEmail:
//                    return "Ya existe un nombre de usuario para esa dirección de correo electrónico. Escriba una dirección de correo electrónico diferente.";

//                case MembershipCreateStatus.InvalidPassword:
//                    return "La contraseña especificada no es válida. Escriba un valor de contraseña válido.";

//                case MembershipCreateStatus.InvalidEmail:
//                    return "La dirección de correo electrónico especificada no es válida. Compruebe el valor e inténtelo de nuevo.";

//                case MembershipCreateStatus.InvalidAnswer:
//                    return "La respuesta de recuperación de la contraseña especificada no es válida. Compruebe el valor e inténtelo de nuevo.";

//                case MembershipCreateStatus.InvalidQuestion:
//                    return "La pregunta de recuperación de la contraseña especificada no es válida. Compruebe el valor e inténtelo de nuevo.";

//                case MembershipCreateStatus.InvalidUserName:
//                    return "El nombre de usuario especificado no es válido. Compruebe el valor e inténtelo de nuevo.";

//                case MembershipCreateStatus.ProviderError:
//                    return "El proveedor de autenticación devolvió un error. Compruebe los datos especificados e inténtelo de nuevo. Si el problema continúa, póngase en contacto con el administrador del sistema.";

//                case MembershipCreateStatus.UserRejected:
//                    return "La solicitud de creación de usuario se ha cancelado. Compruebe los datos especificados e inténtelo de nuevo. Si el problema continúa, póngase en contacto con el administrador del sistema.";

//                default:
//                    return "Error desconocido. Compruebe los datos especificados e inténtelo de nuevo. Si el problema continúa, póngase en contacto con el administrador del sistema.";
//            }
//        }
//        #endregion

//        [AllowAnonymous]
//        public ActionResult Roles()
//        {

//            //System.Web.Security.Roles.CreateRole("atleta");
//            //System.Web.Security.Roles.CreateRole("entrenador");

//            //if (!System.Web.Security.Roles.RoleExists("administrator"))
//            //{
//            //    System.Web.Security.Roles.CreateRole("administrator");
//            //}
//            System.Web.Security.Roles.AddUserToRole("ddo88", "administrator");
//            return Json("{ok:true}", JsonRequestBehavior.AllowGet);
//        }
    }
}
