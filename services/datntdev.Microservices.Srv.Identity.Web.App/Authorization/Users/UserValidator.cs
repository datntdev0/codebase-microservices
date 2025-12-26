using datntdev.Microservices.Srv.Identity.Contract.Authorization.Users.Dto;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using static datntdev.Microservices.Common.Constants.Enum;

namespace datntdev.Microservices.Srv.Identity.Web.App.Authorization.Users
{
    internal class UserCreateValidator : AbstractValidator<UserCreateDto>
    {
        private readonly SrvIdentityDbContext _dbContext;

        public UserCreateValidator(IServiceProvider services)
        {
            _dbContext = services.GetRequiredService<SrvIdentityDbContext>();

            RuleFor(x => x.FirstName).NotEmpty();
            RuleFor(x => x.LastName).NotEmpty();
            RuleFor(x => x.Password).NotEmpty();
            RuleFor(x => x.Permissions).Must(CheckPermissions);

            RuleFor(x => x.EmailAddress)
                .NotEmpty()
                .EmailAddress()
                .Must(CheckDuplicatedEmaill).WithMessage(v => $"The email '{v}' is already existed.");
            RuleFor(x => x.Username)
                .NotEmpty()
                .Must(CheckDuplicatedUsername).WithMessage(v => $"The username '{v}' is already existed.");
        }

        public bool CheckDuplicatedUsername(string username)
        {
            return !_dbContext.AppUsers.Any(u => u.Username == username && !u.IsDeleted);
        }

        public bool CheckDuplicatedEmaill(string emailAddress)
        {
            return !_dbContext.AppUsers.Any(u => u.EmailAddress == emailAddress && !u.IsDeleted);
        }

        public static bool CheckPermissions(AppPermission[] permissions)
        {
            if (permissions.Length == 0) return true;
            return !permissions.Any(x => !Enum.IsDefined(x));
        }
    }

    internal class UserUpdateValidator : AbstractValidator<(long, UserUpdateDto)>
    {
        private readonly SrvIdentityDbContext _dbContext;

        public UserUpdateValidator(IServiceProvider services)
        {
            _dbContext = services.GetRequiredService<SrvIdentityDbContext>();

            RuleFor(x => x.Item2.FirstName).NotEmpty();
            RuleFor(x => x.Item2.LastName).NotEmpty();
            RuleFor(x => x.Item2.RemovePermissions).Must(CheckPermissions);
            RuleFor(x => x.Item2.AppendPermissions).Must(CheckPermissions);

            RuleFor(x => x.Item2.EmailAddress)
                .NotEmpty()
                .EmailAddress()
                .Must((req, email) => CheckDuplicatedEmaill(req.Item1, email))
                .WithMessage(v => $"The email '{v}' is already existed.");
            RuleFor(x => x.Item2.Username)
                .NotEmpty()
                .Must((req, username) => CheckDuplicatedUsername(req.Item1, username))
                .WithMessage(v => $"The username '{v}' is already existed.");
        }
        public bool CheckDuplicatedUsername(long id, string username)
        {
            return !_dbContext.AppUsers.Any(u => u.Id != id && u.Username == username && !u.IsDeleted);
        }
        public bool CheckDuplicatedEmaill(long id, string emailAddress)
        {
            return !_dbContext.AppUsers.Any(u => u.Id != id && u.EmailAddress == emailAddress && !u.IsDeleted);
        }
        public static bool CheckPermissions(AppPermission[] permissions)
        {
            if (permissions.Length == 0) return true;
            return !permissions.Any(x => !Enum.IsDefined(x));
        }
    }
}
