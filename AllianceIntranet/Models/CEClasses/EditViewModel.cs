using System.ComponentModel.DataAnnotations;
using AllianceIntranet.Data.Entities;
using AllianceIntranet.Models.CEClasses;

namespace AllianceIntranet.Models.CEClasses
{
    public class EditViewModel
    {
        public EditViewModel()
        {
        }

        public EditViewModel(CEClass ceClass)
        {
            Date = ceClass.Date;
            Time = ceClass.Time;
            Instructor = ceClass.Instructor;
            Type = ceClass.Type;
            ClassTitle = ceClass.ClassTitle;
            Description = ceClass.Description;
        }

        [Display(Name = "Date")]
        public string Date { get; set; }
        
        [Display(Name = "Time")]
        public string Time { get; set; }
        
        [Display(Name = "Instructor")]
        public string Instructor { get; set; }
        
        [Display(Name = "Type")]
        public ClassType Type { get; set; }
        
        [Display(Name = "Class Title")]
        public string ClassTitle { get; set; }
        
        [Display(Name = "Description")]
        public string Description { get; set; }
    }
}