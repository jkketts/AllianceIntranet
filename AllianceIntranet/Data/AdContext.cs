
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
        public DbSet<RegisteredAgent> RegisteredAgents { get; set; }
        public DbSet<OpenHouse> OpenHouses { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<RegisteredAgent>()
                .HasKey(t => new { t.AppUserId, t.CEClassId });

            builder.Entity<RegisteredAgent>()
                .HasOne(t => t.CEClass)
                .WithMany(t => t.RegisteredAgents)
                .HasForeignKey(t => t.CEClassId);

            builder.Entity<RegisteredAgent>()
                .HasOne(t => t.AppUser)
                .WithMany(t => t.RegisteredAgents)
                .HasForeignKey(t => t.AppUserId);

            base.OnModelCreating(builder);
        }
    }
}
