namespace datntdev.Microservices.Common.Models
{
    public class ErrorResponse
    {
        public int StatusCode { get; set; }
        public int ErrorCode { get; set; }
        public string Message { get; set; } = string.Empty;
        public string? Details { get; set; }
    }
}
