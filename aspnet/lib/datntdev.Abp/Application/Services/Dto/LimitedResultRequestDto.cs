using System.ComponentModel.DataAnnotations;

namespace datntdev.Abp.Application.Services.Dto
{
    /// <summary>
    /// Simply implements <see cref="ILimitedResultRequest"/>.
    /// </summary>
    public class LimitedResultRequestDto : ILimitedResultRequest
    {
        public static int DefaultMaxResultCount { get; set; } = 10;

        [Range(1, int.MaxValue)]
        public virtual int MaxResultCount { get; set; } = DefaultMaxResultCount;
    }
}