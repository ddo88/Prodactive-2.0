using System.Collections.Generic;
using Zeitgeist.Core.Data.Interfaces;

namespace Zeitgeist.Web.Domain
{
    public class Plan : IEntity
    {
        public string Name           { get; set; }

        public int UsuariosAdmitidos { get; set; }

        public virtual ICollection<Liga> Ligas { get; set; }

    }
}