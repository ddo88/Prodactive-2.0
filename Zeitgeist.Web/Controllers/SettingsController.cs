using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Zeitgeist.Core.Data.Interfaces;
using Zeitgeist.Web.Domain;
using Zeitgeist.Web.Services;
using Zeitgeist.Web.Tools;

namespace Zeitgeist.Web.Controllers
{
    public class SettingsController : Controller
    {
        private readonly Security _security;
        private readonly IUserService _userService;
        private readonly PictureService _pictureService;
        private readonly IRepository<User> _userRepository;
        private readonly Media _media;
        //
        // GET: /Settings/

        public SettingsController(Security security,IUserService userService,PictureService pictureService,IRepository<User> userRepository,Media media)
        {
            _security = security;
            _userService = userService;
            _pictureService = pictureService;
            _userRepository = userRepository;
            _media = media;
        }

        public ActionResult UploadAvatar()
        {
            return View();
        }


        public ActionResult FileUpload(HttpPostedFileBase file)
        {
            if (file != null)
            {
                var user = _security.GetAuthenticatedUser();
                // save the image path path to the database or you can send image 
                // directly to database
                // in-case if you want to store byte[] ie. for DB
                byte[] array = null;
                using (MemoryStream ms = new MemoryStream())
                {
                    file.InputStream.CopyTo(ms);
                    array = ms.GetBuffer();
                }

                //var elm=_userRepository.GetById(user.Id);
                Picture p = new Picture();
                p.Bytes = array;
                p.MimeType = file.ContentType;
                p.FileName = file.FileName;
                user.Avatar= p;
                
                //_userRepository.Update(elm);
                _userService.Update(user);
                _media.SavePictureInFile(p.Id, p.Bytes, p.MimeType);

            }
            // after successfully uploading redirect the user
            return RedirectToAction("Index", "Home");
        }



    }
}
