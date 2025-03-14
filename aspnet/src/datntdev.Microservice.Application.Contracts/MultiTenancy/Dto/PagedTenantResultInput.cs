﻿using datntdev.Abp.Application.Services.Dto;
using datntdev.Abp.Runtime.Validation;

namespace datntdev.Microservice.MultiTenancy.Dto;

public class PagedTenantResultInput : PagedResultRequestDto, IShouldNormalize
{
    public string Keyword { get; set; } = string.Empty;
    public string Sorting { get; set; } = string.Empty;
    public bool? IsActive { get; set; }


    public void Normalize()
    {
        if (string.IsNullOrEmpty(Sorting))
        {
            Sorting = "TenancyName,Name";
        }

        Keyword = Keyword.Trim();
    }
}

