using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace AllianceIntranet.Data.Entities
{
    public class AppUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public string Street { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }
        public DateTime LastModified { get; set; }

        public string Office { get; set; }
        public virtual ICollection<Ad> Ads { get; set; }
        public ICollection<RegisteredAgent> RegisteredAgents { get; set; } = new List<RegisteredAgent>();//Watch how to create many to many relationships
    }
}
