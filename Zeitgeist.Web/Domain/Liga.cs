using System.Collections.Generic;
using Zeitgeist.Core.Data.Interfaces;

namespace Zeitgeist.Web.Domain
{
    public class Liga : IEntity
    {
        public int OwnerId { get; set; }
        public string Name { get; set; }
        public int PlanId { get; set; }

        public virtual User User { get; set; }
        public virtual Plan Plan { get; set; }

        
        public virtual ICollection<Division> Divisiones { get; set; }
        public virtual ICollection<User>     Usuarios { get; set; }
        public virtual ICollection<Chat>     Chats { get; set; }
        public virtual ICollection<Reto> Retos { get; set; }
    }
}