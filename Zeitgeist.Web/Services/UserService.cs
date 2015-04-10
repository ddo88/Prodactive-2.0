using System;
using System.Linq;
using Zeitgeist.Core.Data.Interfaces;
using Zeitgeist.Web.Domain;
using Zeitgeist.Web.Tools;

namespace Zeitgeist.Web.Services
{
    public class UserService : IUserService
    {
        private readonly IRepository<User> _userRepository;
        private readonly InMemoryCache _memoryCache;
        
        public UserService(
            IRepository<User> userRepository, 
            InMemoryCache memoryCache)
        {
            _userRepository = userRepository;
            _memoryCache = memoryCache;
        }


        public User GetCustomerByUsername(string user)
        {

            return _memoryCache.GetOrSet(user, () =>
            {
                var query = _userRepository.Table.Where(x => x.UserName == user);
                if (query.Count() > 0)
                    return query.First();

                return new NullUser();
            });
        }

        public void Update(User user)
        {
            if(user==null)
                throw new ArgumentNullException("user");

            _userRepository.Update(user);
            
            _memoryCache.DeleteCache(user.UserName);
            _memoryCache.DeleteCache("UserContext" + user.UserName);
            
        }

        public void SaveSetting(User user, Setting setting,string value)
        {
            UserSettingsMapping us = new UserSettingsMapping();
            us.User                = user;
            us.Setting             = setting;
            us.Value               = value;
            user.UserSettings.Add(us);
            Update(user);
        }
    }
}
