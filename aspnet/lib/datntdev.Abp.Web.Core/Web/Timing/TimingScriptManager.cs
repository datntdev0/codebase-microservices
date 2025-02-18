using System;
using System.Globalization;
using System.Text;
using System.Threading.Tasks;
using datntdev.Abp.Configuration;
using datntdev.Abp.Dependency;
using datntdev.Abp.Extensions;
using datntdev.Abp.Timing;
using datntdev.Abp.Timing.Timezone;

namespace datntdev.Abp.Web.Timing
{
    /// <summary>
    /// This class is used to build timing script.
    /// </summary>
    public class TimingScriptManager : ITimingScriptManager, ITransientDependency
    {
        private readonly ISettingManager _settingManager;

        public TimingScriptManager(ISettingManager settingManager)
        {
            _settingManager = settingManager;
        }

        public async Task<string> GetScriptAsync()
        {
            var script = new StringBuilder();

            script.AppendLine("(function(){");

            script.AppendLine("    abp.clock.provider = abp.timing." + Clock.Provider.GetType().Name.ToCamelCase() + " || abp.timing.localClockProvider;");
            script.AppendLine("    abp.clock.provider.supportsMultipleTimezone = " + Clock.SupportsMultipleTimezone.ToString().ToLowerInvariant() + ";");

            if (Clock.SupportsMultipleTimezone)
            {
                script.AppendLine("    abp.timing.timeZoneInfo = " + await GetUsersTimezoneScriptsAsync());
            }

            script.Append("})();");

            return script.ToString();
        }

        private async Task<string> GetUsersTimezoneScriptsAsync()
        {
            var timezoneId = await _settingManager.GetSettingValueAsync(TimingSettingNames.TimeZone);
            var timezone = TimezoneHelper.FindTimeZoneInfo(timezoneId);

            return " {" +
                   "        windows: {" +
                   "            timeZoneId: '" + timezoneId + "'," +
                   "            baseUtcOffsetInMilliseconds: '" + timezone.BaseUtcOffset.TotalMilliseconds + "'," +
                   "            currentUtcOffsetInMilliseconds: '" + timezone.GetUtcOffset(Clock.Now).TotalMilliseconds + "'," +
                   "            isDaylightSavingTimeNow: '" + timezone.IsDaylightSavingTime(Clock.Now) + "'" +
                   "        }," +
                   "        iana: {" +
                   "            timeZoneId:'" + TimezoneHelper.WindowsToIana(timezoneId) + "'" +
                   "        }," +
                   "    }";
        }
    }
}