using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AllianceIntranet.Data.Entities
{
    public class CEClass
    {
        public int CEClassId { get; set; }
        public string Date { get; set; }
        public string Time { get; set; }
        public string Instructor { get; set; }
        public ClassType Type { get; set; }
        public string ClassTitle { get; set; }
        public string Description { get; set; }

        public virtual AppUser AppUser { get; set; }
    }

    public enum ClassType
    {
        Core,
        Elective,
        Ethics
    }
}
