using AllianceIntranet.Data.Entities;
using AllianceIntranet.Models.OpenHouseSubmission;
using System;
using System.ComponentModel.DataAnnotations;

namespace AllianceIntranet.Models.EmailTemplates
{
    public class EmailOpenHouseViewModel
    {
        public EmailOpenHouseViewModel()
        {
        }

        public EmailOpenHouseViewModel(OpenHouseViewModel model, AppUser appUser)
        {
            DateSubmitted = DateTime.Now;
            MLSNumber = model.MLSNumber;
            Street = model.Street;
            City = model.City;
            Day = model.Day;
            TimeOpen = model.TimeOpen;
            Price = model.Price;
            AgentSubmitted = String.Format($"{appUser.FirstName} {appUser.LastName}");
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
        [Required]
        public string AgentSubmitted { get; set; }
    }

}