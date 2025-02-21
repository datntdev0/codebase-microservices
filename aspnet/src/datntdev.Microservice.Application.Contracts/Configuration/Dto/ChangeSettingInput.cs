using System.ComponentModel.DataAnnotations;

namespace datntdev.Microservice.Configuration.Dto;

public class ChangeSettingInput
{
    [Required]
    [StringLength(32)]
    public string Value { get; set; }
}
