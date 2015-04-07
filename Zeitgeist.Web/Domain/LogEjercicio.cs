using System;
using Zeitgeist.Core.Data.Interfaces;

namespace Zeitgeist.Web.Domain
{
    public class LogEjercicio : IEntity
    {
        public int             UserId    { get; set; }
        public DateTime        Fecha     { get; set; }
        public string          Ubicacion { get; set; }
        public int             DeporteId { get; set; }
        public decimal         Velocidad { get; set; }
        public int             Conteo    { get; set; }
        public virtual User    User      { get; set; }
        public virtual Deporte Deporte   { get; set; }
    }
}