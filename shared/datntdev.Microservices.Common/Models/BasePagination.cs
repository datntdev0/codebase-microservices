namespace datntdev.Microservices.Common.Models
{
    public interface IPaginatedRequest
    {
        public int Offset { get; set; }
        public int Limit { get; set; }
    }

    public interface IPaginatedResult<TData>
    {
        public int Total { get; set; }
        public int Limit { get; set; }
        public int Offset { get; set; }
        public IEnumerable<TData> Items { get; set; }
    }

    public class PaginatedRequest : IPaginatedRequest
    {
        public int Offset { get; set; } = 1;
        public int Limit { get; set; } = 10;
    }

    public class PaginatedResult<TData> : IPaginatedResult<TData>
    {
        public int Total { get; set; }
        public int Limit { get; set; }
        public int Offset { get; set; }
        public IEnumerable<TData> Items { get; set; } = [];
    }
}
