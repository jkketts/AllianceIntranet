
using AllianceIntranet.Data.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AllianceIntranet.Data
{
    public class AdContext : IdentityDbContext<AppUser>
    {
        public AdContext(DbContextOptions<AdContext> options) : base(options)
        {
        }

//        public DbSet<Agent> Agents { get; set; }

        public DbSet<Ad> Ads { get; set; }
        public DbSet<CEClass> CEClasses { get; set; }
    }
}
