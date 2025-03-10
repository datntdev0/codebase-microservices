﻿using datntdev.Abp.Application.Services;
using datntdev.Microservice.MultiTenancy.Dto;

namespace datntdev.Microservice.MultiTenancy;

public interface ITenantsAppService : IAsyncCrudAppService<TenantDto, int, PagedTenantResultInput, CreateTenantInput, TenantDto>
{
}

