using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AllianceIntranet.Data;
using AllianceIntranet.Data.Entities;
using AllianceIntranet.Models;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace AllianceIntranet
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = BuildWebHost(args);

            using (var scope = host.Services.CreateScope())
            {

            var services = scope.ServiceProvider;
                try
                {
                    var context = services.GetRequiredService<AdContext>();
                    var hosting = services.GetRequiredService<IHostingEnvironment>();
                    var userManager = services.GetRequiredService<UserManager<AppUser>>();
                    var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();

                    context.Database.EnsureCreated();

                    if (!userManager.Users.Any())
                    {
                        //Need to create sample data
                        var filepath = Path.Combine(hosting.ContentRootPath, "Data/MOCK_DATA.json");
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
                                Email = newAppUser.Email
                            };
                            userManager.CreateAsync(newUser, newAppUser.Password).Wait();
                        }
                    }
                    
                    IdentityResult roleResult;
                    //Adding Addmin Role    
                    var roleCheck = roleManager.RoleExistsAsync("Admin").Result;
                    if (!roleCheck)
                    {
                        //create the roles and seed them to the database    
                        roleResult = roleManager.CreateAsync(new IdentityRole("Admin")).Result;
                    }

                    roleCheck = roleManager.RoleExistsAsync("Agent").Result;
                    if (!roleCheck)
                    {
                        //create the roles and seed them to the database    
                        roleResult = roleManager.CreateAsync(new IdentityRole("Agent")).Result;
                    }

                    //Assign Admin role to the main User here we have given our newly loregistered login id for Admin management    
                    AppUser user1 = userManager.FindByEmailAsync("justin.ketterman@bhhsall.com").Result;
                    userManager.AddToRoleAsync(user1, "Admin").Wait();

                } 
                    catch (Exception ex)
                {
                    var logger = services.GetRequiredService<ILogger<Program>>();
                    logger.LogError(ex, "Shit's borked.");
                }
            }

                host.Run();
            //BuildWebHost(args).Run();

        }

        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration(SetupConfiguration)
                .UseStartup<Startup>()
                .Build();

        private static void SetupConfiguration(WebHostBuilderContext ctx, IConfigurationBuilder builder)
        {
            //Removes default config options
            builder.Sources.Clear();
            builder.AddJsonFile("config.json", false, true);
        }
    }
}
