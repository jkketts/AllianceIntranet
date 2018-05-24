using AllianceIntranet.Data.Entities;
using Microsoft.AspNetCore.Identity;
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

        public IEnumerable<Ad> GetAdsByUser(string id)
        {
            var ads = from a in _context.Ads
                      where a.AppUser.Id == id
                      select a;

            return ads.ToList();
        }

        public IEnumerable<Ad> GetAllAds()
        {
            var ads = _context.Ads.ToList();
            return ads;
        }

        public IEnumerable<AppUser> GetAllAppUsers()
        {
            return _userManager.Users.ToList();
        }

        public IEnumerable<CEClass> GetAllClasses()
        {
            var classes = _context.CEClasses.ToList();
            return classes;
        }

        public bool SaveChanges()
        {
            return _context.SaveChanges() > 0;
        }

    }
}
