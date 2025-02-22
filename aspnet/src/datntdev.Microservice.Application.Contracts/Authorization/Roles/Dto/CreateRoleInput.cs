using datntdev.Abp.Authorization.Roles;
using System.ComponentModel.DataAnnotations;

namespace datntdev.Microservice.Authorization.Roles.Dto;

public class CreateRoleInput
{
    [Required]
    [StringLength(AbpRoleBase.MaxNameLength)]
    public string Name { get; set; }

    [Required]
    [StringLength(AbpRoleBase.MaxDisplayNameLength)]
    public string DisplayName { get; set; }

    public string NormalizedName { get; set; }

    [StringLength(Role.MaxDescriptionLength)]
    public string Description { get; set; }

    public List<string> GrantedPermissions { get; set; }

    public CreateRoleInput()
    {
        GrantedPermissions = new List<string>();
    }
}
