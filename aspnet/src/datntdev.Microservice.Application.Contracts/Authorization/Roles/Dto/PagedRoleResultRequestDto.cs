using datntdev.Abp.Application.Services.Dto;
using datntdev.Abp.Runtime.Validation;

namespace datntdev.Microservice.Roles.Dto;

public class PagedRoleResultRequestDto : PagedResultRequestDto, IShouldNormalize
{
    public string Keyword { get; set; }
    public string Sorting { get; set; }

    public void Normalize()
    {
        if (string.IsNullOrEmpty(Sorting))
        {
            Sorting = "Name,DisplayName";
        }

        Keyword = Keyword?.Trim();
    }
}

