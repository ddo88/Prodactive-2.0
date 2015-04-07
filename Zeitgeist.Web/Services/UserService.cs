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
            var query = _userRepository.Table.Where(x => x.UserName == user);
            if (query.Count() > 0)
                return query.First();
            
            return new NullUser();
        }

        public void Update(User user)
        {
            if(user==null)
                throw new ArgumentNullException("user");

            _userRepository.Update(user);
            _memoryCache.DeleteCache(user.UserName);
        }
    }


    public class PictureService //: IPictureService
    {
        private readonly IRepository<Picture> _pictureRepository;

        public PictureService(IRepository<Picture> pictureRepository )
        {
            _pictureRepository = pictureRepository;
        }

        public void Insert(Picture picture)
        {
            if(picture==null)
                throw new ArgumentNullException("picture");
            _pictureRepository.Insert(picture);
        }
    }
}
