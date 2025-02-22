using datntdev.Abp.Localization;
using datntdev.Microservice.Models.Session;
using datntdev.Abp.Runtime.Session;
using Microsoft.AspNetCore.Identity;
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace datntdev.Microservice.Controllers
{
    [Route("api/session")]
    public class SessionController : MicroserviceControllerBase
    {
        [HttpGet]
        public async Task<GetCurrentSessionOutput> GetCurrentSessionAsync()
        {
            var output = new GetCurrentSessionOutput
            {
                Application = new ApplicationInfoDto
                {
                    Version = MicroserviceConsts.Version,
                    ReleaseDate = MicroserviceConsts.ReleaseDate,
                    Features = [],
                }
            };

            if (AbpSession.TenantId.HasValue)
            {
                output.Tenant = ObjectMapper.Map<TenantLoginInfoDto>(await GetCurrentTenantAsync());
            }

            if (AbpSession.UserId.HasValue)
            {
                output.User = ObjectMapper.Map<UserLoginInfoDto>(await GetCurrentUserAsync());
            }

            return output;
        }

        [HttpPost("language")]
        public async Task ChangeLanguageAsync([FromBody] ChangeLanguageInput input)
        {
            await SettingManager.ChangeSettingForUserAsync(
                AbpSession.ToUserIdentifier(),
                LocalizationSettingNames.DefaultLanguage,
                input.LanguageName
            );
        }

        [HttpPost("password")]
        public async Task<bool> ChangePasswordAsync([FromBody] ChangePasswordInput input)
        {
            await UserManager.InitializeOptionsAsync(AbpSession.TenantId);

            var user = await UserManager.FindByIdAsync(AbpSession.GetUserId().ToString())
                ?? throw new Exception("There is no current user!");

            if (await UserManager.CheckPasswordAsync(user, input.CurrentPassword))
            {
                CheckErrors(await UserManager.ChangePasswordAsync(user, input.NewPassword));
            }
            else
            {
                CheckErrors(IdentityResult.Failed(new IdentityError
                {
                    Description = "Incorrect password."
                }));
            }

            return true;
        }
    }
}
