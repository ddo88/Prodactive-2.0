using Zeitgeist.Core.Data.Interfaces;

namespace Zeitgeist.Web.Domain
{
    public class UserSettingsMapping:IEntity
    {
        public int UserId { get; set; }
        public int SettingId { get; set; }
        public string Value { get; set; }
        public virtual User User { get; set; }
        public virtual Setting Setting { get; set; }


    }
}