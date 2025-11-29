using datntdev.Microservices.Common.Models;

namespace datntdev.Microservices.Common.Application
{
    public interface IAppService { }

    public interface IAppService<TKey, TDto, TCreateReq, TUpdateReq> : IAppService
        where TKey : IEquatable<TKey>
        where TDto : BaseDto<TKey>
        where TCreateReq : class
        where TUpdateReq : class
    {
        Task<TDto> GetAsync(TKey id);
        Task<TDto> CreateAsync(TCreateReq request);
        Task<TDto> UpdateAsync(TKey id, TUpdateReq request);
        Task DeleteAsync(TKey id);
    }
}
