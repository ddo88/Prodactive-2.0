using Zeitgeist.Core.Data.Interfaces;

namespace Zeitgeist.Web.Domain
{
    public class RetoEquiposMapping:IEntity
    {
        public int RetoId { get; set; }
        public Reto Reto { get; set; }
        public int EquipoId { get; set; }
        public Equipo Equipo { get; set; }

    }
}