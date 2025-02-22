using datntdev.Abp.Auditing;
using datntdev.Abp.Authorization.Users;
using System.ComponentModel.DataAnnotations;

namespace datntdev.Microservice.Models.Auth
{
    public class LoginInput
    {
        [Required]
        [StringLength(AbpUserBase.MaxEmailAddressLength)]
        public string UserNameOrEmailAddress { get; set; }

        [Required]
        [StringLength(AbpUserBase.MaxPlainPasswordLength)]
        [DisableAuditing]
        public string Password { get; set; }

        public bool RememberClient { get; set; }
    }
}
