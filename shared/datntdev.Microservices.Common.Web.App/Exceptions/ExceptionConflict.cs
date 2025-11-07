namespace datntdev.Microservices.Common.Web.App.Exceptions
{
    public class ExceptionConflict : ExceptionBase
    {
        public ExceptionConflict() { }

        public ExceptionConflict(string? message) : base(message) { }

        public ExceptionConflict(string? message, Exception? innerException) : base(message, innerException) { }
    }
}
