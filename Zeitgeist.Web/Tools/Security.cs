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
    public class Security
    {
        private readonly HttpContextBase _httpContext;
        private readonly IUserService _userService;
        private readonly InMemoryCache _memoryCache;

        public Security(HttpContextBase httpContext,
                        IUserService    userService,
                        InMemoryCache memoryCache)
        {
            _httpContext = httpContext;
            _userService = userService;
            _memoryCache = memoryCache;
        }

        private User _cachedUser;

        public virtual User GetAuthenticatedUser()
        {
            //if (_cachedUser != null)
            //    return _cachedUser;

            if (_httpContext == null ||
                _httpContext.Request == null ||
                !_httpContext.Request.IsAuthenticated ||
                !(_httpContext.User.Identity is FormsIdentity))
            {
                return null;
            }

            var formsIdentity = (FormsIdentity)_httpContext.User.Identity;
            var customer = GetAuthenticatedCustomerFromTicket(formsIdentity.Ticket);
            if (customer != null)//&& customer.Active && !customer.Deleted && customer.IsRegistered())
                _cachedUser = customer;
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

            //var customer = _memoryCache.GetOrSet(usernameOrEmail, () =>
            //{
                return _userService.GetCustomerByUsername(usernameOrEmail);
            //});
            
            //var customer = _userService.GetCustomerByUsername(usernameOrEmail);

            //return customer;
        }


        public static class AppRoles
        {
            public readonly static string Admin     = "Admin";
            public readonly static string Athlete   = "Athlete";
            public readonly static string Coach     = "Coach";
        }
    }
    
}
