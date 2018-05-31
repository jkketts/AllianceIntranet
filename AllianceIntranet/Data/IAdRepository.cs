using System.Collections.Generic;
using AllianceIntranet.Data.Entities;

namespace AllianceIntranet.Data
{
    public interface IAdRepository
    {
        //       IEnumerable<Agent> GetAllAgents();
        IEnumerable<AppUser> GetAllAppUsers();
        ICollection<Ad> GetAllAds();
        void AddEntity(object model);
        bool SaveChanges();
        ICollection<Ad> GetAdsByUser(string id);
        ICollection<CEClass> GetAllClasses();
        CEClass GetClassById(int id);
        ICollection<RegisteredAgent> GetRegisteredAgents();
        
    }
}