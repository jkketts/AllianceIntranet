using System.ComponentModel.DataAnnotations;

namespace AllianceIntranet.Models.Account
{
    public class ForgotPasswordViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
