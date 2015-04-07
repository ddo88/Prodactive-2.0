using System.Collections.Generic;
using Zeitgeist.Core.Data.Interfaces;

namespace Zeitgeist.Web.Domain
{
    public class Deporte : IEntity
    {
        public string Name { get; set; }

        public int TipoConteoId { get; set; }

        public TipoConteo TipoConteo {
            get
            {
                return (TipoConteo)this.TipoConteoId;
            }
            set
            {
                this.TipoConteoId = (int)value;
            }
        }

        public int FactorConteo { get; set; }

        public string Tips { get; set; }
        public virtual ICollection<LogEjercicio> LogEjercicios { get; set; }
        public virtual ICollection<RetoDeportesMapping> RetoDeporteMapping { get; set; }
    }
}