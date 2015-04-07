using System.Collections.Generic;
using Zeitgeist.Core.Data.Interfaces;

namespace Zeitgeist.Web.Domain
{
    public class TipoReto : IEntity
    {
        public string Name { get; set; }
        public ICollection<Reto> Retos { get; set; }

    }
}