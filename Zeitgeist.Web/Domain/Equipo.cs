using System.Collections.Generic;
using Zeitgeist.Core.Data.Interfaces;

namespace Zeitgeist.Web.Domain
{
    public class Equipo : IEntity
    {
        public string Name { get; set; }

        public virtual ICollection<UserEquipoMapping> MiembrosEquipoMapping { get; set; }
        public virtual ICollection<EquipoDivisionMapping> EquipoDivisionMapping { get; set; }
        public virtual ICollection<RetoEquiposMapping> RetosEquipoMapping { get; set; }
    }
}