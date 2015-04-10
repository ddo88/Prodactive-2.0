using System.Web;
using Ninject.Activation;
using Ninject.Syntax;

namespace Zeitgeist.Web.Ninject
{
    public static class NinjectSessionScopingExtention
    {
        public static void InSessionScope<T>(this IBindingInSyntax<T> parent)
        {
            parent.InScope(SessionScopeCallback);
        }

        private const string _sessionKey = "ZeitgeistWeb";

        private static object SessionScopeCallback(IContext context)
        {
            if (HttpContext.Current.Session[_sessionKey] == null)
            {
                HttpContext.Current.Session[_sessionKey] = new object();
            }

            return HttpContext.Current.Session[_sessionKey];
        }
    }
}