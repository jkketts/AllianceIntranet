using AllianceIntranet.Data.Entities;
using System.ComponentModel.DataAnnotations;

namespace AllianceIntranet.Models.EmailTemplates
{
    public class EmailRegisterViewModel
    {
        public EmailRegisterViewModel()
        {
        }

        public EmailRegisterViewModel(CEClass ceClass)
        {
            Date = ceClass.Date;
            Time = ceClass.Time;
            Instructor = ceClass.Instructor;
            Type = ceClass.Type;
            ClassTitle = ceClass.ClassTitle;
            Description = ceClass.Description;
        }

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
    }

}