using System.ComponentModel.DataAnnotations;

namespace datntdev.Microservice.Users.Dto;

public class ChangeUserLanguageDto
{
    [Required]
    public string LanguageName { get; set; }
}