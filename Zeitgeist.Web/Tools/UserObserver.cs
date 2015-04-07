using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zeitgeist.Core.Patterns.Observer;
using Zeitgeist.Web.Hubs;
using Zeitgeist.Web.Models.Page;

namespace Zeitgeist.Web.Tools
{
    public class UserObserver : Core.Patterns.Observer.IObserver<Notification>
    {
        private readonly string _connectionId;
        private readonly Action<string, Notification> _action;

        public UserObserver(string username, string connectionId, Action<string, Notification> action)
        {
            _connectionId = connectionId;
            _action = action;
            Id = username;
        }

        public string Id { get; set; }
        public void Notify(Notification message)
        {
            _action(_connectionId, message);
        }
    }
}
