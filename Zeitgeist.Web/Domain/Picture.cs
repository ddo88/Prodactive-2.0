using System.Collections.Generic;
using Zeitgeist.Core.Data.Interfaces;

namespace Zeitgeist.Web.Domain
{
    public class Picture : IEntity
    {
        public string FileName { get; set; }
        public byte[] Bytes      { get; set; }
        public string MimeType   { get; set; }
        public virtual ICollection<Tip> Tips { get; set; }

        public virtual ICollection<User> Users { get; set; }

    }
}