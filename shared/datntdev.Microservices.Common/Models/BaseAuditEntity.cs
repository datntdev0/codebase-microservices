namespace datntdev.Microservices.Common.Models
{
    public class BaseEntity<TKey> where TKey : IEquatable<TKey>
    {
        public TKey Id { get; set; } = default!;
    }

    public class BaseAuditEntity<TKey>
        : BaseEntity<TKey>, ICreated, IUpdated where TKey : IEquatable<TKey>
    {
        public DateTime? CreatedAt { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string? UpdatedBy { get; set; }
        public string ConcurrencyStamp { get; set; } = Guid.NewGuid().ToString();
    }

    public class FullAuditEntity<TKey>
        : BaseAuditEntity<TKey>, IDeleted where TKey : IEquatable<TKey>
    {
        public bool IsDeleted { get; set; }
    }
}
