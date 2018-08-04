using System;
using System.ComponentModel.DataAnnotations;

namespace AllianceIntranet.Models.Account
{
    public class MakeAdminViewModel
    {
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

    }
}