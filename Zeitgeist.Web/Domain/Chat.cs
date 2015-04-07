using System;
using Zeitgeist.Core.Data.Interfaces;

namespace Zeitgeist.Web.Domain
{
    public class Chat:IEntity
    {
        
        public int      LigaId      { get; set; }
        public int      UserId   { get; set; }
        public string   Message     { get; set; }
        public DateTime PublishDate { get; set; }

        public virtual User User    { get; set; }
        public virtual Liga Liga    { get; set; }
    }
}