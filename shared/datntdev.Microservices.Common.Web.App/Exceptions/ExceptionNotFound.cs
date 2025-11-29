
namespace datntdev.Microservices.Common.Web.App.Exceptions
{
    public class ExceptionNotFound : ExceptionBase
    {
        public ExceptionNotFound() { }

        public ExceptionNotFound(string? message) : base(message) { }

        public ExceptionNotFound(string? message, Exception? innerException) : base(message, innerException) { }
    }
}
