using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR;
using Zeitgeist.Core.Data.Interfaces;
using Zeitgeist.Web.Domain;

namespace Zeitgeist.Web.Services
{
    public class ChatService
    {
        private readonly IRepository<Chat> _chatRepository;
        private readonly IUserService _userService;

        public ChatService(IRepository<Chat> chatRepository, IUserService userService)
        {
            _chatRepository = chatRepository;
            _userService    = userService;
        }

        public void Save(string message,string userName)
        {
            var user = _userService.GetCustomerByUsername(userName);

            Chat chat= new Chat();
            chat.User = user;
            chat.Message = message;
            chat.PublishDate = DateTime.UtcNow;
            _chatRepository.Insert(chat);
            
        }

        //private static void SendTohub(string command, string parameter)
        //{
        //    var hubContext = GlobalHost.ConnectionManager.GetHubContext<HubService>();
        //    hubContext.Clients.All.command(command, parameter);

        //}
    }
}
