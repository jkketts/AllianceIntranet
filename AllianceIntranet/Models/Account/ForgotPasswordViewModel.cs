using System.ComponentModel.DataAnnotations;

namespace AllianceIntranet.Models
{
    public class ForgotPasswordViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
