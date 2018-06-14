using AllianceIntranet.Data.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AllianceIntranet.Models.AdSubmission
{
    public class AdViewModel
    {
        [Required]
        [RegularExpression("([0-9]+)", ErrorMessage = "Please enter a valid number.")]
        [Range(10000000, 100000000, ErrorMessage = "MLS Number must be 8 digits.")]
        [Display(Name = "MLS Number")]
        public int MLSNumber { get; set; }

        [Required]
        [Display(Name = "Street")]
        public string Street { get; set; }

        [Required]
        [Display(Name = "City")]
        public string City { get; set; }

        [Required]
        [RegularExpression("(^\\d+(\\.\\d+)?$)", ErrorMessage = "Please enter valid a number.")]
        [Display(Name = "Price")]
        public decimal Price { get; set; }

        [Required]
        public string AppUserId { get; set; }
    }
}