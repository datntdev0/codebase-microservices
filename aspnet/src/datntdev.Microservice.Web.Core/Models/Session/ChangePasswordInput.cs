using System.ComponentModel.DataAnnotations;

namespace datntdev.Microservice.Models.Session;

public class ChangePasswordInput
{
    [Required]
    public string CurrentPassword { get; set; }

    [Required]
    public string NewPassword { get; set; }
}
