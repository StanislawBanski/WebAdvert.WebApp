using System.ComponentModel.DataAnnotations;

namespace WebAdvert.Web.Models.Accounts
{
    public class ConfirmModel
    {
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress]
        [Display(Name ="Email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Code is required")]
        [Display(Name = "Code")]
        public string Code { get; set; }
    }
}
