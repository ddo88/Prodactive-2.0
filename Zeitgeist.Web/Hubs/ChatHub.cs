using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using Ninject;
using Ninject.Activation;
using Zeitgeist.Core.Data.Interfaces;
using Zeitgeist.Web.Domain;
using Zeitgeist.Web.Services;
using Zeitgeist.Web.Tools;
using UrlHelper = ServiceStack.Html.UrlHelper;

namespace Zeitgeist.Web.Hubs
{
    [HubName("Chat")]
    public class ChatHub : Hub
    {
        public override Task OnConnected()
        {
            var repository=(IRepository<User>) GlobalConfiguration.Configuration.DependencyResolver.GetService(typeof (IRepository<User>));
            //ligas 
            var user = repository.Table.Where(x => x.UserName == this.Context.User.Identity.Name).First();
            foreach (var liga in user.Ligas)
                Groups.Add(this.Context.ConnectionId, liga.Name);
            foreach (var equipos in user.UserMapping)
                Groups.Add(this.Context.ConnectionId, equipos.Equipo.Name);
            
            return base.OnConnected();
        }

        public override Task OnDisconnected(bool stopCalled)
        {
            var repository = (IRepository<User>)GlobalConfiguration.Configuration.DependencyResolver.GetService(typeof(IRepository<User>));
            //ligas 
            var user = repository.Table.Where(x => x.UserName == this.Context.User.Identity.Name).First();
            foreach (var liga in user.Ligas)
                Groups.Remove(this.Context.ConnectionId, liga.Name);
            foreach (var equipos in user.UserMapping)
                Groups.Remove(this.Context.ConnectionId, equipos.Equipo.Name);

            return base.OnDisconnected(stopCalled);
        }


        //public void SendToTeam(string team,string message)
        //{
        //    //var avatar = GetAvatar();
        //    //Clients.Group(team).broadcastMessage(this.Context.User.Identity.Name, message, avatar);
        //}

        //public void SendToLeague(string league,string message)
        //{
        //    //var avatar = GetAvatar();
        //    //Clients.Group(league).broadcastMessage(this.Context.User.Identity.Name, message, avatar);
        //}
        public void SendToBroadcast(string message)
        {
            try
            {
                var avatar = GetAvatar();
                var t = Task.Factory.StartNew(() =>
                {
                    ChatService service =
                        (ChatService)GlobalConfiguration.Configuration.DependencyResolver.GetService(typeof(ChatService));
                    service.Save(message, this.Context.User.Identity.Name);
                });

                var result = new ChatMessage()
                {
                    UserName = this.Context.User.Identity.Name,
                    Message = message,
                    Avatar = avatar
                };
                Clients.All.BroadcastMessage(result);
                t.Wait();
            }
            catch (Exception)
            {
                
                throw;
            }
            
        }

        public void Otrho()
        {
            Clients.All.Otrho();
        }

        private string GetAvatar()
        {
            Media re      = (Media)GlobalConfiguration.Configuration.DependencyResolver.GetService(typeof(Media));
            var avatar    = re.GetUserAvatarUrl(this.Context.User.Identity.Name);
            var urlHelper = new UrlHelper();
            avatar        = urlHelper.Content(avatar);
            return avatar;
        }

    }

    public class ChatMessage
    {
        public string UserName { get; set; }
        public string Avatar { get; set; }
        public string Message { get; set; }

        public string PublishDate { get; set; }
    }
}