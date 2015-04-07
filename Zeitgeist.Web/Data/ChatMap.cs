using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading.Tasks;
using Humanizer;
using Zeitgeist.Core.Data.Configuration;
using Zeitgeist.Web.Domain;

namespace Zeitgeist.Web.Data
{
    public class ChatMap : ZeitgeistEntityTypeConfiguration<Chat>
    {
        public ChatMap()
        {
            ToTable("Chat");
            HasKey(e => e.Id);

            Property(e => e.Message);
            Property(e => e.PublishDate);

            HasRequired(e => e.User)
                .WithMany(f => f.Chats)
                .HasForeignKey(e => e.UserId);

            HasRequired(e => e.Liga)
                .WithMany(f => f.Chats)
                .HasForeignKey(e => e.LigaId);
        }
    }
}
