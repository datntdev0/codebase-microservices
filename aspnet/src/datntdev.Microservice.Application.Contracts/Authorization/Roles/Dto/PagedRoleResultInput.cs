using datntdev.Abp.Application.Services.Dto;
using datntdev.Abp.Runtime.Validation;

namespace datntdev.Microservice.Authorization.Roles.Dto;

public class PagedRoleResultInput : PagedResultRequestDto, IShouldNormalize
{
    public string Keyword { get; set; } = string.Empty;
    public string Sorting { get; set; } = string.Empty;

    public void Normalize()
    {
        if (string.IsNullOrEmpty(Sorting))
        {
            Sorting = "Name,DisplayName";
        }

        Keyword = Keyword.Trim();
    }
}

