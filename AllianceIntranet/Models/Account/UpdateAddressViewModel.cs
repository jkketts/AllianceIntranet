using AllianceIntranet.Data.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AllianceIntranet.Models.Account
{
    public class UpdateAddressViewModel
    {
        public UpdateAddressViewModel() { }
        public UpdateAddressViewModel(AppUser user)
        {
            FirstName = user.FirstName;
            LastName = user.LastName;
            Street = user.Street;
            City = user.City;
            State = user.State;
            Zip = user.Zip;
            Phone = user.PhoneNumber;
        }

        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Required]
        [Display(Name = "Street")]
        public string Street { get; set; }

        [Required]
        [Display(Name = "City")]
        public string City { get; set; }

        [Required]
        [Display(Name = "State")]
        public string State { get; set; }

        [Required]
        [Display(Name = "Zip")]
        public string Zip { get; set; }

        [Required]
        [Display(Name = "Phone")]
        public string Phone { get; set; }
    }
}
