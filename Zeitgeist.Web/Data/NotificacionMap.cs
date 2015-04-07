using Zeitgeist.Core.Data.Configuration;
using Zeitgeist.Web.Domain;

namespace Zeitgeist.Web.Data
{
    public class NotificacionMap : ZeitgeistEntityTypeConfiguration<Notificacion>
    {
        public NotificacionMap()
        {
            ToTable("Notificacion");
            HasKey(e => e.Id);
            HasRequired(e => e.User)
                .WithMany(f => f.UserNotifications)
                .HasForeignKey(e => e.IdUser)
                .WillCascadeOnDelete(false);

            //Property(e => e.PublishDate).HasColumnType("datetime");

        }
    }
}