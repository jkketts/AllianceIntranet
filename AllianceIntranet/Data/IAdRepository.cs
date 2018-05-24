using System.Collections.Generic;
using AllianceIntranet.Data.Entities;

namespace AllianceIntranet.Data
{
    public interface IAdRepository
    {
        //       IEnumerable<Agent> GetAllAgents();
        IEnumerable<AppUser> GetAllAppUsers();
        IEnumerable<Ad> GetAllAds();
        void AddEntity(object model);
        bool SaveChanges();
        IEnumerable<Ad> GetAdsByUser(string id);
        IEnumerable<CEClass> GetAllClasses();

    }
}