using System.ComponentModel.DataAnnotations;

namespace SimpleNotesApp.ViewModels
{
    public class ForgotPasswordViewModel
    {
        [Required(ErrorMessage = "Email address is mandatory")]
        [EmailAddress(ErrorMessage = "Enter a valid email address")]
        [Display(Name = "Email")]
        public string? Email {get; set;}
    }
} 