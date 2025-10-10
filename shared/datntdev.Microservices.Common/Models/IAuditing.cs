namespace datntdev.Microservices.Common.Models
{
    public interface ICreated
    {
        public DateTime? CreatedAt { get; set; }
        public string? CreatedBy { get; set; }
    }

    public interface IUpdated
    {
        public DateTime? UpdatedAt { get; set; }
        public string? UpdatedBy { get; set; }
    }

    public interface IDeleted
    {
        public bool IsDeleted { get; set; }
    }
}
