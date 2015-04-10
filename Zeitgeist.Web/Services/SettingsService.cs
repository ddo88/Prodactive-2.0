using System.Linq;
using Zeitgeist.Core.Data.Interfaces;
using Zeitgeist.Web.Domain;

namespace Zeitgeist.Web.Services
{
    public class SettingsService
    {
        private readonly IRepository<Setting> _settingRepository;
        private readonly IRepository<UserSettingsMapping> _userSettingRepository;

        public SettingsService(IRepository<Setting> settingRepository, IRepository<UserSettingsMapping> userSettingRepository)
        {
            _settingRepository = settingRepository;
            _userSettingRepository = userSettingRepository;
        }

        public Setting GetSettingByName(string name)
        {
            var query = _settingRepository.Table.Where(x => x.Name.Contains(name)).ToList();
            if(query.Count>=0)
                return query.First();

            return null;
        }

    }
}