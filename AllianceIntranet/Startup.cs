using AllianceIntranet.Data;
using AllianceIntranet.Data.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using AllianceIntranet.Services;

namespace AllianceIntranet
{
    public class Startup
    {
        private readonly IConfiguration _config;
        public Startup(IConfiguration configuration)
        {
            _config = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddEntityFrameworkNpgsql().AddDbContext<AdContext>(cfg =>
            {
                //For connecting to Heroku DB
                cfg.UseNpgsql("Database=deovvmq9sso1r5; host=ec2-54-235-206-118.compute-1.amazonaws.com; Port=5432; User ID=sbnuuypwjytdhm; Password=a1c737eb5f949c05639775a523760895767984bedc73e99d27ba00de4cf7c340; sslmode=Require; Trust Server Certificate=true");
                //For local db work
                //cfg.UseSqlServer(_config.GetConnectionString("AdConnectionString")/*, b=> b.MigrationsAssembly("AllianceIntranet.Data")*/);
            });


            services.AddIdentity<AppUser, IdentityRole>()
                    .AddEntityFrameworkStores<AdContext>()
                    .AddDefaultTokenProviders();

            services.AddTransient<AppUserSeeder>();

            services.AddScoped<IAdRepository, AdRepository>();
            services.AddScoped<IEmailSender, EmailSender>();

            services.AddMvc(config =>
            {
                var policy = new AuthorizationPolicyBuilder()
                                .RequireAuthenticatedUser()
                                .Build();
                config.Filters.Add(new AuthorizeFilter(policy));
            });
            
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {

            app.UseStatusCodePages(async context =>
            {
                context.HttpContext.Response.ContentType = "text/plain";
                await context.HttpContext.Response.WriteAsync(
                    "Sorry, there was an issue. Status code: " +
                    context.HttpContext.Response.StatusCode);
            });

            if (env.IsDevelopment())
            {
                app.UseBrowserLink();
                app.UseDeveloperExceptionPage();
            }
            else
            {

                //app.UseExceptionHandler("/error");
            }

            app.UseStaticFiles();

            app.UseAuthentication();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });

        }

        
    }
}
