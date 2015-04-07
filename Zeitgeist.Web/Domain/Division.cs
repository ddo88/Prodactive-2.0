using System.Collections.Generic;
using Zeitgeist.Core.Data.Interfaces;

namespace Zeitgeist.Web.Domain
{
    public class Division : IEntity
    {
        public string Name { get; set; }

        public virtual ICollection<EquipoDivisionMapping> EquiposDivisionMapping { get; set; }
        public virtual ICollection<Reto> Retos { get; set; }
    }
}