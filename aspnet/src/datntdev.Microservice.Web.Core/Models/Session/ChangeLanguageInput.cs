using System.ComponentModel.DataAnnotations;

namespace datntdev.Microservice.Models.Session;

public class ChangeLanguageInput
{
    [Required]
    public string LanguageName { get; set; }
}