using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Configuration;
using System.Web.Http;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using Zeitgeist.Core.Patterns.Observer;

using Zeitgeist.Web.Domain;
using Zeitgeist.Web.Models.Page;
using Zeitgeist.Web.Tools;
using Notification = Zeitgeist.Web.Models.Page.Notification;

namespace Zeitgeist.Web.Hubs
{

    [HubName("Notifications")]
    public class NotificationsHub : Hub
    {


        public override Task OnConnected()
        {
            ISubject<Notification> re = (ISubject<Notification>)GlobalConfiguration.Configuration
                                                                                   .DependencyResolver
                                                                                   .GetService(typeof(ISubject<Notification>));

            re.Subcribe(new UserObserver(this.Context.User.Identity.Name,this.Context.ConnectionId,(x, y) => { Clients.Client(x).Notify(y); }));
            return base.OnConnected();
        }

        public override Task OnDisconnected(bool stopCalled)
        {
            ISubject<Notification> re = (ISubject<Notification>)GlobalConfiguration.Configuration
                                                                                   .DependencyResolver
                                                                                   .GetService(typeof(ISubject<Notification>));
            re.Unsubscribe(this.Context.User.Identity.Name);
            return base.OnDisconnected(stopCalled);
        }

        public void Test()
        {

            Clients.All.Notify(new Notification()
            {
                Id = 0,
                NotifyType =(int) NotifyType.Avoid,
                Text = "Test",
                IsReaded=false,
                UserName = "UserName"
            });
        }

        public void DeleteNotification(int id)
        {
            Clients.Client(this.Context.ConnectionId).RemoveNotification(id);
            //process to delete 
        }


    }

    

}