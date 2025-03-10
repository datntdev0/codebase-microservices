using datntdev.Abp.Auditing;
using datntdev.Abp.Authorization.Users;
using datntdev.Abp.AutoMapper;
using datntdev.Abp.Runtime.Validation;
using datntdev.Microservice.Authorization.Users;
using System.ComponentModel.DataAnnotations;

namespace datntdev.Microservice.Authorization.Users.Dto;

[AutoMapTo(typeof(User))]
public class CreateUserInput : IShouldNormalize
{
    [Required]
    [StringLength(AbpUserBase.MaxUserNameLength)]
    public string UserName { get; set; }

    [Required]
    [StringLength(AbpUserBase.MaxNameLength)]
    public string Name { get; set; }

    [Required]
    [StringLength(AbpUserBase.MaxSurnameLength)]
    public string Surname { get; set; }

    [Required]
    [EmailAddress]
    [StringLength(AbpUserBase.MaxEmailAddressLength)]
    public string EmailAddress { get; set; }

    public bool IsActive { get; set; }

    public string[] RoleNames { get; set; }

    [Required]
    [StringLength(AbpUserBase.MaxPlainPasswordLength)]
    [DisableAuditing]
    public string Password { get; set; }

    public void Normalize()
    {
        if (RoleNames == null)
        {
            RoleNames = new string[0];
        }
    }
}
