using AllianceIntranet.Models;
using System;
using System.ComponentModel.DataAnnotations;

namespace AllianceIntranet.Data.Entities
{
    public class Ad
    {
        public Ad()
        {
        }

        public Ad(AdViewModel model, AppUser appUser)
        {
            DateSubmitted = DateTime.Now;
            MLSNumber = model.MLSNumber;
            Street = model.Street;
            City = model.City;
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
        public decimal Price { get; set; }

        public virtual AppUser AppUser { get; set; }
    }
}
