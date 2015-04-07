using Zeitgeist.Core.Data.Interfaces;

namespace Zeitgeist.Web.Domain
{
    public class EquipoDivisionMapping : IEntity
    {
        public int DivisionId { get; set; }
        public int EquipoId { get; set; }

        public virtual Division Division { get; set; }
        public virtual Equipo   Equipo { get; set; }
    }
}