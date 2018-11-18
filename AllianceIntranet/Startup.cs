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
using AllianceIntranet.Services;
using System;
using RazorLight;
using System.IO;

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
                Connection connection = new Connection();
                var envVar = Environment.GetEnvironmentVariable("DATABASE_URL");
                var connectionString = connection.GetConnection(envVar);

                cfg.UseNpgsql(connectionString);

            });


            services.AddIdentity<AppUser, IdentityRole>()
                    .AddEntityFrameworkStores<AdContext>()
                    .AddDefaultTokenProviders();

            services.Configure<IdentityOptions>(options =>
            {
                options.Password.RequireDigit = false;
                options.Password.RequiredLength = 4;
                options.Password.RequireLowercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequiredUniqueChars = 1;
            });

            services.AddTransient<AppUserSeeder>();

            services.AddScoped<IAdRepository, AdRepository>();
            services.AddScoped<IEmailSender, EmailSender>();

            services.AddSingleton<IRazorLightEngine>(f =>
            {
                return (new EngineFactory())
                    .ForFileSystem(Path.Combine(Directory.GetCurrentDirectory(), "EmailTemplates"/*, "RegisteredClass.cshtml"*/));
            });

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
