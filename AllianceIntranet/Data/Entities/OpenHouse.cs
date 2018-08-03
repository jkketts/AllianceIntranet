using AllianceIntranet.Models;
using AllianceIntranet.Models.OpenHouseSubmission;
using System;
using System.ComponentModel.DataAnnotations;

namespace AllianceIntranet.Data.Entities
{
    public class OpenHouse
    {
        public OpenHouse()
        {
        }

        public OpenHouse(OpenHouseViewModel model, AppUser appUser)
        {
            DateSubmitted = DateTime.Now;
            MLSNumber = model.MLSNumber;
            Street = model.Street;
            City = model.City;
            Day = model.Day;
            TimeOpen = model.TimeOpen;
            Price = model.Price;
            AppUser = appUser;
        }

        public int Id { get; set; }
        public DateTime DateSubmitted { get; set; }
        [Required]
        public int MLSNumber { get; set; }
        [Required]
        public string Street { get; set; }
        [Required]
        public string City { get; set; }
        [Required]
        public string Day { get; set; }
        [Required]
        public string TimeOpen { get; set; }
        [Required]
        public decimal Price { get; set; }

        public virtual AppUser AppUser { get; set; }
    }
}
