namespace datntdev.Microservices.Srv.Identity.Contract.Authorization.Dto
{
    public class UserUpdateDto
    {
        public string EmailAddress { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string? Password { get; set; }
    }
}
