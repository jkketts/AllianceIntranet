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
        List<RegisteredAgent> GetRegisteredAgents();
        void RemoveRegisteredAgent(RegisteredAgent registeredAgent);
        void RemoveClass(CEClass ceClass);
        
    }
}