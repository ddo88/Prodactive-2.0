using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zeitgeist.Core.Data.Interfaces;
using Zeitgeist.Web.Models.Page;

namespace Zeitgeist.Web.Domain
{
    public class Notificacion:IEntity
    {
        public string Message { get; set; }
        public int NotifyType { get; set; }
        public bool IsReaded{ get; set; }
        public int IdUser   { get; set; }

        public DateTime PublishDate { get; set; }
        public virtual User User { get; set; }
    }

    public enum Rol
    {
        Root,
        Atleta
    }
}
