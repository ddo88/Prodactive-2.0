using Zeitgeist.Core.Data.Configuration;
using Zeitgeist.Web.Domain;

namespace Zeitgeist.Web.Data
{
    public class DivisionMap : ZeitgeistEntityTypeConfiguration<Division>
    {
        public DivisionMap()
        {
            ToTable("Division");
            HasKey(e => e.Id);

            Property(e => e.Name);


        }
    }
}