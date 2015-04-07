using Zeitgeist.Core.Data.Configuration;
using Zeitgeist.Web.Domain;

namespace Zeitgeist.Web.Data
{
    public class TipoRetoMap : ZeitgeistEntityTypeConfiguration<TipoReto>
    {
        public TipoRetoMap()
        {
            ToTable("TipoReto");
            HasKey(e => e.Id);
            Property(e => e.Name);

            
        }
    }
}