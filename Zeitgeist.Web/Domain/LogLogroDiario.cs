using System;
using Zeitgeist.Core.Data.Interfaces;

namespace Zeitgeist.Web.Domain
{
    public class LogLogroDiario : IEntity
    {
        public int RetoId { get; set; }
        public DateTime Fecha { get; set; }
        public int UserId { get; set; }

        //LogroDiario?

        public virtual User User { get; set; }
        public virtual Reto Reto { get; set; }
    }
}