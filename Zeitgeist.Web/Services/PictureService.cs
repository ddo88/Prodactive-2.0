using System;
using Zeitgeist.Core.Data.Interfaces;
using Zeitgeist.Web.Domain;

namespace Zeitgeist.Web.Services
{
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