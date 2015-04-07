using System;
using System.Collections.Generic;
using Zeitgeist.Core.Data.Interfaces;

namespace Zeitgeist.Web.Domain
{
    public class Reto : IEntity
    {
        public string Name { get; set; }
        public int LigaId { get; set; }
        public int DivisionId { get; set; }
        public int OwnerId { get; set; }
        public int CoachId { get; set; }
        public int TipoRetoId { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
        public int Meta { get; set; }
        public bool IsActivo { get; set; }
        public int PremioId { get; set; }
        
        public virtual Premio               Premio { get; set; }
        public virtual TipoReto             TipoReto { get; set; }
        public virtual User                 Coach{ get; set; }
        public virtual User                 Owner { get; set; }
        public virtual Liga                 Liga { get; set; }
        public virtual Division             Division { get; set; }
        public virtual ICollection<RetoDeportesMapping> RetoDeporteMapping { get; set; }
        public virtual ICollection<RetoEquiposMapping> RetoEquipoMapping { get; set; }
        public virtual ICollection<LogLogroDiario> LogLogrosDiarios { get; set; }
    }
}