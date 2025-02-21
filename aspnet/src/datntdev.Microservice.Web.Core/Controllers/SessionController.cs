using datntdev.Abp.Localization;
using datntdev.Microservice.Sessions.Dto;
using datntdev.Abp.Runtime.Session;
using datntdev.Microservice.Users.Dto;
using Microsoft.AspNetCore.Identity;
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace datntdev.Microservice.Controllers
{
    [Route("api/[controller]/[action]")]
    public class SessionController : MicroserviceControllerBase
    {
        [HttpGet]
        public async Task<GetCurrentLoginInformationsOutput> GetCurrentLoginInformations()
        {
            var output = new GetCurrentLoginInformationsOutput
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

        [HttpPost]
        public async Task<bool> ChangePassword([FromBody] ChangePasswordDto input)
        {
            await UserManager.InitializeOptionsAsync(AbpSession.TenantId);

            var user = await UserManager.FindByIdAsync(AbpSession.GetUserId().ToString());
            if (user == null)
            {
                throw new Exception("There is no current user!");
            }

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

        [HttpPost]
        public async Task ChangeLanguage([FromBody] ChangeUserLanguageDto input)
        {
            await SettingManager.ChangeSettingForUserAsync(
                AbpSession.ToUserIdentifier(),
                LocalizationSettingNames.DefaultLanguage,
                input.LanguageName
            );
        }
    }
}
