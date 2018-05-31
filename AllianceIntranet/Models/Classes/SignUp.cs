using AllianceIntranet.Data.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AllianceIntranet.Models.Classes
{
    public class SignUp
    {
        //public SignUp(CEClass ceClass, )

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

        [Display(Name = "Is Registered")]
        public IList<string> RegisteredUsers { get; set; }
    }
}
