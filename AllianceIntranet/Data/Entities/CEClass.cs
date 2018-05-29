using AllianceIntranet.Models;
using System.Collections.Generic;

namespace AllianceIntranet.Data.Entities
{
    public class CEClass
    {
        public CEClass()
        {
        }

        public CEClass(ClassViewModel model)
        {
            Date = model.Date;
            Time = model.Time;
            Instructor = model.Instructor;
            Type = model.Type;
            ClassTitle = model.ClassTitle;
            Description = model.Description;
        }

        public int Id { get; set; }
        public string Date { get; set; }
        public string Time { get; set; }
        public string Instructor { get; set; }
        public ClassType Type { get; set; }
        public string ClassTitle { get; set; }
        public string Description { get; set; }

        public ICollection<RegisteredAgent> RegisteredAgents { get; set; } = new List<RegisteredAgent>();
    }

    public enum ClassType
    {
        Core,
        Elective,
        Ethics
    }
}
