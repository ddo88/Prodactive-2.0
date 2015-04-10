using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Security;
using Zeitgeist.Web.Domain;
using Zeitgeist.Web.Services;

namespace Zeitgeist.Web.Tools
{
    public class WorkContext
    {
        private readonly HttpContextBase _httpContext;
        private readonly IUserService _userService;
        private readonly InMemoryCache _memoryCache;
        private readonly SettingsService _settingsService;

        public WorkContext( HttpContextBase httpContext,
                            IUserService    userService,
                            InMemoryCache memoryCache,
                            SettingsService settingsService)
        {
            _httpContext = httpContext;
            _userService = userService;
            _memoryCache = memoryCache;
            _settingsService = settingsService;
        }

        private UserContext _cachedUser;

        public virtual UserContext GetAuthenticatedUser()
        {
            
            if (_httpContext == null ||
                _httpContext.Request == null ||
                !_httpContext.Request.IsAuthenticated ||
                !(_httpContext.User.Identity is FormsIdentity))
            {
                return null;
            }

            var formsIdentity = (FormsIdentity)_httpContext.User.Identity;
            _cachedUser=_memoryCache.GetOrSet("UserContext" + formsIdentity.Ticket.Name, () =>
            {
                var customer = GetAuthenticatedCustomerFromTicket(formsIdentity.Ticket);
                if (customer != null) //&& customer.Active && !customer.Deleted && customer.IsRegistered())
                {
                    var context  = new UserContext();
                    context.User = customer;
                    context.IdLeague = (int) (customer.GetSetting(SettingBase.League)??0);
                       
                    return context;
                }

                return null;
            });
            
            if (_cachedUser != null)
                return _cachedUser;
    
            _memoryCache.DeleteCache("UserContext" + formsIdentity.Ticket);
            return _cachedUser;
        }


        private User GetAuthenticatedCustomerFromTicket(FormsAuthenticationTicket ticket)
        {
            if (ticket == null)
                throw new ArgumentNullException("ticket");

            //var usernameOrEmail = ticket.UserData;
            var usernameOrEmail = ticket.Name;
            if (String.IsNullOrWhiteSpace(usernameOrEmail))
                return null;
            
            return _userService.GetCustomerByUsername(usernameOrEmail);
        }


        public void SaveUserSetting(string setting,string value)
        {

            var s = _settingsService.GetSettingByName(setting);
            var user = GetAuthenticatedUser();
            _userService.SaveSetting(user.User, s, value);


            _memoryCache.DeleteCache("UserContext" + user.User.UserName);
            _memoryCache.DeleteCache(user.User.UserName);

        }


        public static class AppRoles
        {
            public readonly static string Admin     = "Admin";
            public readonly static string Athlete   = "Athlete";
            public readonly static string Coach     = "Coach";
        }
    }

    public class UserContext
    {
        public User User { get; set; }

        public int IdLeague { get; set; }



    }

}
