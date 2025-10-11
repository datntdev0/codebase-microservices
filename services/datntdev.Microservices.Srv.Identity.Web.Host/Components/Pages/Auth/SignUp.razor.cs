using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using System.ComponentModel.DataAnnotations;

namespace datntdev.Microservices.Srv.Identity.Web.Host.Components.Pages.Auth
{
    public partial class SignUp
    {
        private EditContext _editContext = default!;

        [Inject]
        private NavigationManager NavigationManager { get; set; } = default!;

        [CascadingParameter]
        private HttpContext HttpContext { get; set; } = default!;

        [SupplyParameterFromForm]
        private InputModel Model { get; set; } = new();

        protected override void OnInitialized()
        {
            base.OnInitialized();
            _editContext = new EditContext(Model);
        }

        private string GetInvalidClass(string fieldName)
        {
            var fieldIdentifier = new FieldIdentifier(Model, fieldName);
            return _editContext.IsValid(fieldIdentifier) ? string.Empty : "is-invalid";
        }

        private async Task HandleValidSubmit()
        {
        }

        public class InputModel
        {
            [Required]
            [DataType(DataType.EmailAddress)]
            [EmailAddress(ErrorMessage = "Invalid email address format.")]
            public string? Email { get; set; }

            [Required]
            [DataType(DataType.Text)]
            public string? FirstName { get; set; }

            [Required]
            [DataType(DataType.Text)]
            public string? LastName { get; set; }

            [Required]
            [DataType(DataType.Password)]
            public string? Password { get; set; }

            [Required]
            [DataType(DataType.Password)]
            [Compare(nameof(Password), ErrorMessage = "The password and confirmation password do not match.")]
            public string? ConfirmPassword { get; set; }
        }
    }
}
