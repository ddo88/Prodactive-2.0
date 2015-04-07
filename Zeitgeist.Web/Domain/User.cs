using System.Collections.Generic;
using Zeitgeist.Core.Data.Interfaces;

namespace Zeitgeist.Web.Domain
{
    public class User : IEntity
    {
        public string           UserName    { get; set; }
        public string           Email       { get; set; }
        public int?             AvatarId { get; set; }

        public virtual Persona  Persona     { get; set; }
        public virtual Picture  Avatar      { get; set; }
        
        public virtual ICollection<Chat> Chats { get; set; }
        public virtual ICollection<Liga> Ligas { get; set; }
        public virtual ICollection<UserEquipoMapping> UserMapping { get; set; }
        public virtual ICollection<Invitaciones> Invitaciones { get; set; }
        public virtual ICollection<LogEjercicio> LogEjercicios { get; set; }
        public virtual ICollection<LogLogroDiario> LogLogroDiarios { get; set; }
        public virtual ICollection<Reto> CoachRetos { get; set; }
        public virtual ICollection<Reto> OwnerRetos { get; set; }
        public virtual ICollection<Notificacion> UserNotifications { get; set; }
    }

    public class NullUser:User
    {
        public NullUser()
        {
        }
    }
}