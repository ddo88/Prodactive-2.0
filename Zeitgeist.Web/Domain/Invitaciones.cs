using Zeitgeist.Core.Data.Interfaces;

namespace Zeitgeist.Web.Domain
{
    public class Invitaciones : IEntity
    {
        public int      LigaId  { get; set; }
        public string   Email   { get; set; }
        public bool     Estado  { get; set; }
        public int      UserId  { get; set; }
        
        public virtual User User { get; set; }
        public virtual Liga Liga { get; set; }
    }
}