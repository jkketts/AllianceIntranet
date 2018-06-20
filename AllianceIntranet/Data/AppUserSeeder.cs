using AllianceIntranet.Data.Entities;
using AllianceIntranet.Models.Account;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AllianceIntranet.Data
{
    public class AppUserSeeder
    {
        private readonly AdContext _context;
        private readonly IHostingEnvironment _hosting;
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        

        public AppUserSeeder(AdContext context, IHostingEnvironment hosting, UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _hosting = hosting;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public void Initialize()
        {
            _context.Database.EnsureCreated();

            IdentityResult roleResult;
            //Adding Addmin Role    
            var roleCheck = _roleManager.RoleExistsAsync("Admin").Result;
            if (!roleCheck)
            {
                //create the roles and seed them to the database    
                roleResult = _roleManager.CreateAsync(new IdentityRole("Admin")).Result;
            }

            roleCheck = _roleManager.RoleExistsAsync("Agent").Result;
            if (!roleCheck)
            {
                //create the roles and seed them to the database    
                roleResult = _roleManager.CreateAsync(new IdentityRole("Agent")).Result;
            }

            var users = _userManager.Users.ToList();

            if (!_userManager.Users.Any())
            {
                //Need to create sample data
                var filepath = Path.Combine(_hosting.ContentRootPath, "Data/MOCK_DATA.json");
                var json = File.ReadAllText(filepath);
                var appUsers = JsonConvert.DeserializeObject<IEnumerable<RegisterViewModel>>(json);
                foreach (var newAppUser in appUsers)
                {
                    var newUser = new AppUser
                    {
                        FirstName = newAppUser.FirstName,
                        LastName = newAppUser.LastName,
                        UserName = newAppUser.Email,
                        Office = newAppUser.Office,
                        Email = newAppUser.Email,
                        Street = newAppUser.Street,
                        City = newAppUser.City,
                        State = newAppUser.State,
                        Zip = newAppUser.Zip,
                        PhoneNumber = newAppUser.Phone
                    };
                    newUser.LastModified = System.DateTime.Now.AddDays(-181);
                    _userManager.CreateAsync(newUser, newAppUser.Password).Wait();
                    _userManager.AddToRoleAsync(newUser, "Agent").Wait();
                }
            }

            //Assign Admin role to the main User here we have given our newly loregistered login id for Admin management    
            AppUser admin = _userManager.FindByEmailAsync("justin.ketterman@bhhsall.com").Result;
            _userManager.AddToRoleAsync(admin, "Admin").Wait();
        }
    }
}
