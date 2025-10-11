namespace datntdev.Microservices.Common.Models
{
    public class BaseDto<TKey> where TKey : IEquatable<TKey>
    {
        public TKey Id { get; set; } = default!;
    }

    public class BaseAuditDto<TKey>
        : BaseDto<TKey>, ICreated, IUpdated where TKey : IEquatable<TKey>
    {
        public DateTime? CreatedAt { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string? UpdatedBy { get; set; }
    }
}
