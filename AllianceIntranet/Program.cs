using System;
using AllianceIntranet.Data;
using AllianceIntranet.Data.Entities;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

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

                    var appUserSeeder = new AppUserSeeder(context, hosting, userManager, roleManager);
                    appUserSeeder.Initialize();
                } 
                    catch (Exception ex)
                {
                    var logger = services.GetRequiredService<ILogger<Program>>();
                    logger.LogError(ex, "Seeder failed.");
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
