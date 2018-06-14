using AllianceIntranet.Data.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AllianceIntranet.Data
{
    public class AdRepository : IAdRepository
    {
        private readonly AdContext _context;
        private readonly UserManager<AppUser> _userManager;

        public AdRepository(AdContext context, UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public void AddEntity(object model)
        {
            _context.Add(model);
        }

        public ICollection<Ad> GetAdsByUser(string id)
        {
            var ads = from a in _context.Ads
                      where a.AppUser.Id == id
                      select a;

            return ads
                    .Include(s => s.AppUser)
                    .ToList();
        }

        public ICollection<Ad> GetAllAds()
        {
            var ads = _context.Ads;

            return ads.Include(s => s.AppUser).ToList();
        }

        public IEnumerable<AppUser> GetAllAppUsers()
        {
            return _userManager.Users.ToList();
        }

        public ICollection<CEClass> GetAllClasses()
        {
            var classes = _context.CEClasses.Include(s => s.RegisteredAgents).ToList();
            return classes;
        }

        public CEClass GetClassById(int id)
        {
            var ceClass = from c in _context.CEClasses
                          where c.Id == id
                          select c;

            return ceClass.FirstOrDefault();
        }

        public bool SaveChanges()
        {
            return _context.SaveChanges() > 0;
        }

        public List<RegisteredAgent> GetRegisteredAgents()
        {
            var regAgents = _context.RegisteredAgents.ToList();

            return regAgents;
        }

        public void RemoveRegisteredAgent(RegisteredAgent registeredAgent)
        {
            var agents = _context.RegisteredAgents.Remove(registeredAgent);
        }

        public void RemoveClass(CEClass ceClass)
        {
            var removeClass = _context.CEClasses.Remove(ceClass);
        }

    }
}
