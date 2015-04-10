using Zeitgeist.Core.Data.Configuration;
using Zeitgeist.Web.Domain;

namespace Zeitgeist.Web.Data
{
    public class SettingMap : ZeitgeistEntityTypeConfiguration<Setting>
    {
        public SettingMap()
        {
            ToTable("Setting");
            HasKey(e => e.Id);

            Property(e => e.Name).IsRequired();
            Property(e => e.TypeValue).IsRequired();


        }
    }
}