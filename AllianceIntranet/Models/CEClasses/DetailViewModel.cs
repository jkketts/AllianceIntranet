using AllianceIntranet.Data.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AllianceIntranet.Models.CEClasses
{
    public class DetailViewModel
    {
        public DetailViewModel()
        {
        }

        //We use IEnumerable<AppUser> since we need to get related data, which we can't get from IEnumerable<RegisteredAgent>
        public DetailViewModel(CEClass ceClass, IEnumerable<AppUser> registeredAgents)
        {
            Date = ceClass.Date;
            Time = ceClass.Time;
            Instructor = ceClass.Instructor;
            Type = ceClass.Type;
            ClassTitle = ceClass.ClassTitle;
            Description = ceClass.Description;
            SpotsLeft = (ceClass.MaxSize - registeredAgents.Count());
            RegisteredAgents = registeredAgents;
        }

        [Required]
        [Display(Name = "Class ID")]
        public int ClassID { get; set; }

        [Required]
        [Display(Name = "Date")]
        public string Date { get; set; }

        [Required]
        [Display(Name = "Time")]
        public string Time { get; set; }

        [Required]
        [Display(Name = "Instructor")]
        public string Instructor { get; set; }

        [Required]
        [Display(Name = "Type")]
        public ClassType Type { get; set; }

        [Required]
        [Display(Name = "Class Title")]
        public string ClassTitle { get; set; }

        [Required]
        [Display(Name = "Description")]
        public string Description { get; set; }

        [Required]
        [Display(Name = "Spots Left")]
        public int SpotsLeft { get; set; }

        [Display(Name = "List of Users")]
        public IEnumerable<AppUser> RegisteredAgents { get; set; }
    }
}