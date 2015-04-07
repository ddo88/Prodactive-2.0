using Zeitgeist.Core.Data.Configuration;
using Zeitgeist.Web.Domain;

namespace Zeitgeist.Web.Data
{
    public class PremioMap : ZeitgeistEntityTypeConfiguration<Premio>
    {
        public PremioMap()
        {
            ToTable("Premio");
            HasKey(e => e.Id);

            Property(e => e.Name);
        }
    }
}