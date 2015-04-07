using Zeitgeist.Core.Data.Configuration;
using Zeitgeist.Web.Domain;

namespace Zeitgeist.Web.Data
{
    public class LigaMap : ZeitgeistEntityTypeConfiguration<Liga>
    {
        public LigaMap()
        {
            ToTable("Ligas");
            HasKey(e => e.Id);

            Property(e => e.Name);

            HasRequired(e=>e.User)
                .WithMany(f=>f.Ligas)
                .HasForeignKey(e=>e.OwnerId);

            HasRequired(e => e.Plan)
                .WithMany(f => f.Ligas)
                .HasForeignKey(e => e.PlanId);


        }
    }
}