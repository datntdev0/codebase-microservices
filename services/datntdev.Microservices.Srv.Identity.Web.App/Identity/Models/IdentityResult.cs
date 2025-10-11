namespace datntdev.Microservices.Srv.Identity.Web.App.Identity.Models
{
    public class IdentityResult
    {
        public IdentityResultStatus Status { get; private set; }

        public static IdentityResult Success => new() { Status = IdentityResultStatus.Success };
        public static IdentityResult Failure => new() { Status = IdentityResultStatus.Failure };
        public static IdentityResult Duplicated => new() { Status = IdentityResultStatus.Duplicated };
    }

    public enum IdentityResultStatus
    {
        Success,
        Failure,
        Duplicated,
    }
}
