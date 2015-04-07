using Zeitgeist.Core.Data.Configuration;
using Zeitgeist.Web.Domain;

namespace Zeitgeist.Web.Data
{
    public class DeporteMap : ZeitgeistEntityTypeConfiguration<Deporte>
    {
        public DeporteMap()
        {
            ToTable("Deporte");
            HasKey(e => e.Id);

            Property(e => e.Name);
            Property(e => e.TipoConteoId);
            Property(e => e.FactorConteo);
            Property(e => e.Tips);

            Ignore(e => e.TipoConteo);
        }
    }
}