using datntdev.Abp.Application.Services.Dto;
using datntdev.Abp.Authorization.Users;
using datntdev.Abp.AutoMapper;
using datntdev.Microservice.Authorization.Users;
using System;
using System.ComponentModel.DataAnnotations;

namespace datntdev.Microservice.Authorization.Users.Dto;

[AutoMapFrom(typeof(User))]
public class UserDto : EntityDto<long>
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

    public string FullName { get; set; }

    public DateTime? LastLoginTime { get; set; }

    public DateTime CreationTime { get; set; }

    public string[] RoleNames { get; set; }
}
