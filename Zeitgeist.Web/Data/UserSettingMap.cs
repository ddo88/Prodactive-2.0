using Zeitgeist.Core.Data.Configuration;
using Zeitgeist.Web.Domain;

namespace Zeitgeist.Web.Data
{
    public class UserSettingMap : ZeitgeistEntityTypeConfiguration<UserSettingsMapping>
    {
        public UserSettingMap()
        {
            ToTable("UserSettings");
            HasKey(e => e.Id);

            HasRequired(e => e.User).WithMany(f => f.UserSettings).HasForeignKey(e => e.UserId);
            HasRequired(e => e.Setting).WithMany(f => f.UserSettings).HasForeignKey(e => e.SettingId);

        }
    }
}