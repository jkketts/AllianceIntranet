using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AllianceIntranet.Data.Entities
{
    public class Ad
    {
        public int AdId { get; set; }
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
