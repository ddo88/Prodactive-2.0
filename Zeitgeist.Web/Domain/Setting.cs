using System.Collections.Generic;
using Zeitgeist.Core.Data.Interfaces;

namespace Zeitgeist.Web.Domain
{
    public class Setting:IEntity
    {
        public string Name { get; set; }
        public int TypeValue { get; set; }

        public virtual ICollection<UserSettingsMapping> UserSettings { get; set; }
    }

    public static class SettingType
    {

        public static int IntValue = 0;
        public static int StringValue = 1;


    }

    public static class SettingBase
    {
        public static string League = "DefaultLeague";
    }
}