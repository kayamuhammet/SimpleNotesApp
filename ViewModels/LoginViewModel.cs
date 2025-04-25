using System.ComponentModel.DataAnnotations;

namespace SimpleNotesApp.ViewModels
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "E-mail address is required")]
        [EmailAddress(ErrorMessage = "Enter a valid e-mail address")]
        public string? Email { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        public string? Password { get; set; }

        [Display(Name = "Beni hatırla")]
        public bool RememberMe { get; set; }
    }
}