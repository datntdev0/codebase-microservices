using System.Collections.Generic;
using System.Threading.Tasks;
using datntdev.Abp.Application.Features;
using datntdev.Abp.Application.Navigation;
using datntdev.Abp.Authorization;
using datntdev.Abp.Configuration;
using datntdev.Abp.Configuration.Startup;
using datntdev.Abp.Localization;
using datntdev.Abp.Runtime.Session;
using datntdev.Abp.Timing;
using datntdev.Abp.Timing.Timezone;
using datntdev.Abp.Web.Models.AbpUserConfiguration;
using datntdev.Abp.Web.Security.AntiForgery;
using System.Linq;
using datntdev.Abp.Dependency;
using datntdev.Abp.Extensions;
using System.Globalization;

namespace datntdev.Abp.Web.Configuration
{
    public class AbpUserConfigurationBuilder : ITransientDependency
    {
        private readonly IAbpStartupConfiguration _startupConfiguration;

        protected IMultiTenancyConfig MultiTenancyConfig { get; }
        protected ILanguageManager LanguageManager { get; }
        protected ILocalizationManager LocalizationManager { get; }
        protected IFeatureManager FeatureManager { get; }
        protected IFeatureChecker FeatureChecker { get; }
        protected IPermissionManager PermissionManager { get; }
        protected IUserNavigationManager UserNavigationManager { get; }
        protected ISettingDefinitionManager SettingDefinitionManager { get; }
        protected ISettingManager SettingManager { get; }
        protected IAbpAntiForgeryConfiguration AbpAntiForgeryConfiguration { get; }
        protected IAbpSession AbpSession { get; }
        protected IPermissionChecker PermissionChecker { get; }
        protected Dictionary<string, object> CustomDataConfig { get; }

        private readonly IIocResolver _iocResolver;

        public AbpUserConfigurationBuilder(
            IMultiTenancyConfig multiTenancyConfig,
            ILanguageManager languageManager,
            ILocalizationManager localizationManager,
            IFeatureManager featureManager,
            IFeatureChecker featureChecker,
            IPermissionManager permissionManager,
            IUserNavigationManager userNavigationManager,
            ISettingDefinitionManager settingDefinitionManager,
            ISettingManager settingManager,
            IAbpAntiForgeryConfiguration abpAntiForgeryConfiguration,
            IAbpSession abpSession,
            IPermissionChecker permissionChecker,
            IIocResolver iocResolver,
            IAbpStartupConfiguration startupConfiguration)
        {
            MultiTenancyConfig = multiTenancyConfig;
            LanguageManager = languageManager;
            LocalizationManager = localizationManager;
            FeatureManager = featureManager;
            FeatureChecker = featureChecker;
            PermissionManager = permissionManager;
            UserNavigationManager = userNavigationManager;
            SettingDefinitionManager = settingDefinitionManager;
            SettingManager = settingManager;
            AbpAntiForgeryConfiguration = abpAntiForgeryConfiguration;
            AbpSession = abpSession;
            PermissionChecker = permissionChecker;
            _iocResolver = iocResolver;
            _startupConfiguration = startupConfiguration;

            CustomDataConfig = new Dictionary<string, object>();
        }

        public virtual async Task<AbpUserConfigurationDto> GetAll()
        {
            return new AbpUserConfigurationDto
            {
                MultiTenancy = GetUserMultiTenancyConfig(),
                Session = GetUserSessionConfig(),
                Localization = GetUserLocalizationConfig(),
                Features = await GetUserFeaturesConfig(),
                Auth = await GetUserAuthConfig(),
                Nav = await GetUserNavConfig(),
                Setting = await GetUserSettingConfig(),
                Clock = GetUserClockConfig(),
                Timing = await GetUserTimingConfig(),
                Security = GetUserSecurityConfig(),
                Custom = _startupConfiguration.GetCustomConfig()
            };
        }

        protected virtual AbpMultiTenancyConfigDto GetUserMultiTenancyConfig()
        {
            return new AbpMultiTenancyConfigDto
            {
                IsEnabled = MultiTenancyConfig.IsEnabled,
                IgnoreFeatureCheckForHostUsers = MultiTenancyConfig.IgnoreFeatureCheckForHostUsers
            };
        }

        protected virtual AbpUserSessionConfigDto GetUserSessionConfig()
        {
            return new AbpUserSessionConfigDto
            {
                UserId = AbpSession.UserId,
                TenantId = AbpSession.TenantId,
                ImpersonatorUserId = AbpSession.ImpersonatorUserId,
                ImpersonatorTenantId = AbpSession.ImpersonatorTenantId,
                MultiTenancySide = AbpSession.MultiTenancySide
            };
        }

        protected virtual AbpUserLocalizationConfigDto GetUserLocalizationConfig()
        {
            var currentCulture = CultureInfo.CurrentUICulture;
            var languages = LanguageManager.GetActiveLanguages();

            var config = new AbpUserLocalizationConfigDto
            {
                CurrentCulture = new AbpUserCurrentCultureConfigDto
                {
                    Name = currentCulture.Name,
                    DisplayName = currentCulture.DisplayName
                },
                Languages = languages.ToList()
            };

            if (languages.Count > 0)
            {
                config.CurrentLanguage = LanguageManager.CurrentLanguage;
            }

            var sources = LocalizationManager.GetAllSources().OrderBy(s => s.Name).ToArray();
            config.Sources = sources.Select(s => new AbpLocalizationSourceDto
            {
                Name = s.Name,
                Type = s.GetType().Name
            }).ToList();

            config.Values = new Dictionary<string, Dictionary<string, string>>();
            foreach (var source in sources)
            {
                var stringValues = source.GetAllStrings(currentCulture).OrderBy(s => s.Name).ToList();
                var stringDictionary = stringValues
                    .ToDictionary(_ => _.Name, _ => _.Value);
                config.Values.Add(source.Name, stringDictionary);
            }

            return config;
        }

        protected virtual async Task<AbpUserFeatureConfigDto> GetUserFeaturesConfig()
        {
            var config = new AbpUserFeatureConfigDto()
            {
                AllFeatures = new Dictionary<string, AbpStringValueDto>()
            };

            var allFeatures = FeatureManager.GetAll().ToList();

            if (AbpSession.TenantId.HasValue)
            {
                var currentTenantId = AbpSession.GetTenantId();
                foreach (var feature in allFeatures)
                {
                    var value = await FeatureChecker.GetValueAsync(currentTenantId, feature.Name);
                    config.AllFeatures.Add(feature.Name, new AbpStringValueDto
                    {
                        Value = value
                    });
                }
            }
            else
            {
                foreach (var feature in allFeatures)
                {
                    config.AllFeatures.Add(feature.Name, new AbpStringValueDto
                    {
                        Value = feature.DefaultValue
                    });
                }
            }

            return config;
        }

        protected virtual async Task<AbpUserAuthConfigDto> GetUserAuthConfig()
        {
            var config = new AbpUserAuthConfigDto();

            var allPermissionNames = PermissionManager.GetAllPermissions(false).Select(p => p.Name).ToList();
            var grantedPermissionNames = new List<string>();

            if (AbpSession.UserId.HasValue)
            {
                foreach (var permissionName in allPermissionNames)
                {
                    if (await PermissionChecker.IsGrantedAsync(permissionName))
                    {
                        grantedPermissionNames.Add(permissionName);
                    }
                }
            }

            config.AllPermissions = allPermissionNames.ToDictionary(permissionName => permissionName, permissionName => "true");
            config.GrantedPermissions = grantedPermissionNames.ToDictionary(permissionName => permissionName, permissionName => "true");

            return config;
        }

        protected virtual async Task<AbpUserNavConfigDto> GetUserNavConfig()
        {
            var userMenus = await UserNavigationManager.GetMenusAsync(AbpSession.ToUserIdentifier());
            return new AbpUserNavConfigDto
            {
                Menus = userMenus.ToDictionary(userMenu => userMenu.Name, userMenu => userMenu)
            };
        }

        protected virtual async Task<AbpUserSettingConfigDto> GetUserSettingConfig()
        {
            var config = new AbpUserSettingConfigDto
            {
                Values = new Dictionary<string, string>()
            };

            var settings = await SettingManager.GetAllSettingValuesAsync(SettingScopes.All);

            using (var scope = _iocResolver.CreateScope())
            {
                foreach (var settingValue in settings)
                {
                    if (!await SettingDefinitionManager.GetSettingDefinition(settingValue.Name).ClientVisibilityProvider
                        .CheckVisible(scope))
                    {
                        continue;
                    }

                    config.Values.Add(settingValue.Name, settingValue.Value);
                }
            }

            return config;
        }

        protected virtual AbpUserClockConfigDto GetUserClockConfig()
        {
            return new AbpUserClockConfigDto
            {
                Provider = Clock.Provider.GetType().Name.ToCamelCase()
            };
        }

        protected virtual async Task<AbpUserTimingConfigDto> GetUserTimingConfig()
        {
            var timezoneId = await SettingManager.GetSettingValueAsync(TimingSettingNames.TimeZone);
            var timezone = TimezoneHelper.FindTimeZoneInfo(timezoneId);

            return new AbpUserTimingConfigDto
            {
                TimeZoneInfo = new AbpUserTimeZoneConfigDto
                {
                    Windows = new AbpUserWindowsTimeZoneConfigDto
                    {
                        TimeZoneId = timezoneId,
                        BaseUtcOffsetInMilliseconds = timezone.BaseUtcOffset.TotalMilliseconds,
                        CurrentUtcOffsetInMilliseconds = timezone.GetUtcOffset(Clock.Now).TotalMilliseconds,
                        IsDaylightSavingTimeNow = timezone.IsDaylightSavingTime(Clock.Now)
                    },
                    Iana = new AbpUserIanaTimeZoneConfigDto
                    {
                        TimeZoneId = TimezoneHelper.WindowsToIana(timezoneId)
                    }
                }
            };
        }

        protected virtual AbpUserSecurityConfigDto GetUserSecurityConfig()
        {
            return new AbpUserSecurityConfigDto
            {
                AntiForgery = new AbpUserAntiForgeryConfigDto
                {
                    TokenCookieName = AbpAntiForgeryConfiguration.TokenCookieName,
                    TokenHeaderName = AbpAntiForgeryConfiguration.TokenHeaderName
                }
            };
        }
    }
}
