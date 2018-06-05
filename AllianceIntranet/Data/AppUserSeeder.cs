using AllianceIntranet.Data.Entities;
using AllianceIntranet.Models.Account;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace AllianceIntranet.Data
{
    public class AppUserSeeder
    {
        private readonly AdContext _ctx;
        private readonly IHostingEnvironment _hosting;
        private readonly UserManager<AppUser> _userManager;

        public AppUserSeeder(AdContext ctx, IHostingEnvironment hosting, UserManager<AppUser> userManager)
        {
            _ctx = ctx;
            _hosting = hosting;
            _userManager = userManager;
        }

        public void Seed()
        {
            _ctx.Database.EnsureCreated();

            if (!_userManager.Users.Any()) {
                //Need to create sample data
                var filepath = Path.Combine(_hosting.ContentRootPath, "Data/MOCK_DATA.json");
                var json = File.ReadAllText(filepath);
                var appUsers = JsonConvert.DeserializeObject<IEnumerable<RegisterViewModel>>(json);
                foreach (var user in appUsers)
                {
                    var newUser = new AppUser
                    {
                        FirstName = user.FirstName,
                        LastName = user.LastName,
                        UserName = user.Email,
                        Email = user.Email
                    };
                    var result = _userManager.CreateAsync(newUser, user.Password);
                }
                //_userManager.Users.AddRange(appUsers);

                //_ctx.SaveChanges();
            }
        }

        public async Task CreateUserRoles(IServiceProvider serviceProvider)
        {
            var RoleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var UserManager = serviceProvider.GetRequiredService<UserManager<AppUser>>();

            IdentityResult roleResult;
            //Adding Addmin Role    
            var roleCheck = await RoleManager.RoleExistsAsync("Admin");
            if (!roleCheck)
            {
                //create the roles and seed them to the database    
                roleResult = await RoleManager.CreateAsync(new IdentityRole("Admin"));
            }

            roleCheck = await RoleManager.RoleExistsAsync("Agent");
            if (!roleCheck)
            {
                //create the roles and seed them to the database    
                roleResult = await RoleManager.CreateAsync(new IdentityRole("Agent"));
            }

            //Assign Admin role to the main User here we have given our newly loregistered login id for Admin management    
            AppUser user = await UserManager.FindByEmailAsync("justin@ketterman.tv");
            var User = new AppUser();
            await UserManager.AddToRoleAsync(user, "Admin");

            user = await UserManager.FindByEmailAsync("afi.ahmed@gmail.com");
            await UserManager.AddToRoleAsync(user, "Agent");

        }

    }
}
