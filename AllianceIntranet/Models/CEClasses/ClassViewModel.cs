using AllianceIntranet.Data.Entities;
using System.ComponentModel.DataAnnotations;

namespace AllianceIntranet.Models.CEClasses
{
    public class ClassViewModel
    {
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
        [Display(Name = "Max Size")]
        public int MaxSize { get; set; }

    }

}