using Zeitgeist.Web.Domain;

namespace Zeitgeist.Web.Services
{
    public interface IUserService
    {
        User GetCustomerByUsername(string user);
        void Update(User user);
    }
}