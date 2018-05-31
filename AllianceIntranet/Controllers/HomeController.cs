using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using AllianceIntranet.Data.Entities;
using AllianceIntranet.Models;
using AllianceIntranet.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using System.Net.Mail;
using Microsoft.Extensions.Logging;
using Microsoft.Exchange.WebServices.Data;
using AllianceIntranet.Models.Classes;

namespace AllianceIntranet.Controllers
{
    public class HomeController : Controller
    {
        private readonly IAdRepository _repo;
        private readonly UserManager<AppUser> _userManager;
        private readonly ILogger<HomeController> _logger;

        public HomeController(IAdRepository repo, UserManager<AppUser> userManager, ILogger<HomeController> logger)
        {
            _repo = repo;
            _userManager = userManager;
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult AdSubmission()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AdSubmission(AdViewModel model)
        {
            if (ModelState.IsValid)
            {
                AppUser AppUser = await _userManager.FindByIdAsync(model.AppUserId);

                var ad = new Ad(model, AppUser);

                _repo.AddEntity(ad);
                _repo.SaveChanges();

                ModelState.Clear();
                return Redirect("/Home/AdSuccess");
            }
            /* Read http://blog.staticvoid.co.nz/2012/entity_framework-navigation_property_basics_with_code_first/ */
            return View();
        }

        public IActionResult AdSuccess()
        {
            return View();
        }

        [HttpGet]
        public IActionResult AppUsers()
        {
            var agents = _repo.GetAllAppUsers();

            return View(agents);
        }

        [HttpGet("Ads/{id}")]
        public IActionResult Ads(string id)
        {
            if (User.IsInRole("Admin"))
            {
                ICollection<Ad> ads = _repo.GetAllAds();

                return View(ads);
            }
            else
            {
                ICollection<Ad> ads = _repo.GetAdsByUser(id);

                return View(ads);
            }
        }

        public IActionResult Ads()
        {
            var ads = _repo.GetAllAds();

            return View(ads);
        }

        public IActionResult Register()
        {
            return View();
        }

        public IActionResult AddClass()
        {
            return View();
        }

        public IActionResult Classes()
        {
            var ceClasses = _repo.GetAllClasses();
        
            return View(ceClasses);
        }

        [HttpPost]
        public IActionResult AddClass(ClassViewModel model)
        {
            if (ModelState.IsValid)
            {
                var ceClass = new CEClass(model);

                _repo.AddEntity(ceClass);
                _repo.SaveChanges();

                ModelState.Clear();
                return Redirect("/Home/Classes");
            }

            return View();
        }

        [HttpPost("RegisterClass/{id}")]
        public async Task<IActionResult> RegisterClass(int id)
        {
            var ceClass = _repo.GetClassById(id);

            var currentUser = await _userManager.GetUserAsync(HttpContext.User);

            ceClass.RegisteredAgents.Add( new RegisteredAgent { AppUser = currentUser, CEClass = ceClass });

            _repo.SaveChanges();
                      
            return Redirect("/Home/Classes");
        }        

    }
}
