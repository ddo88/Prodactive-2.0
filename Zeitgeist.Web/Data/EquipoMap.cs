using Zeitgeist.Core.Data.Configuration;
using Zeitgeist.Web.Domain;

namespace Zeitgeist.Web.Data
{
    public class EquipoMap : ZeitgeistEntityTypeConfiguration<Equipo>
    {
        public EquipoMap()
        {
            ToTable("Equipo");
            HasKey(e => e.Id);
            Property(e => e.Name);
            
        }
    }
}