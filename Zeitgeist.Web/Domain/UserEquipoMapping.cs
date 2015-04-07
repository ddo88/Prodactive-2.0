using Zeitgeist.Core.Data.Interfaces;

namespace Zeitgeist.Web.Domain
{
    public class UserEquipoMapping : IEntity
    {
        public int UserId   { get; set; }
        public int EquipoId { get; set; }

        public virtual Equipo Equipo { get; set; }
        public virtual User   User   { get; set; }

    }
}