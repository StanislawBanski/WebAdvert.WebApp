using System.ComponentModel.DataAnnotations;

namespace WebAdvert.Web.Models.Accounts
{
    public class SingupModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [MinLength(8,ErrorMessage ="Password must be at least eight characters long!")]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage ="Passwords must match")]
        [MinLength(8, ErrorMessage = "Password must be at least eight characters long!")]
        [Display(Name = "Password")]
        public string ConfirmedPassword { get; set; }
    }
}
